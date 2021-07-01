// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Azure;
using Azure.AI.TextAnalytics;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing
{
    public static class AudioProcessing
    {
        [FunctionName("AudioProcessing")]
        public static async Task RunAsync([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            //Extracting content type and url of the blob triggering the function
            var jsondata = JsonConvert.SerializeObject(eventGridEvent.Data);
            var tmp = new { contentType = "", url = "" };
            var data = JsonConvert.DeserializeAnonymousType(jsondata, tmp);

            //Checking if the trigger was iniatiated for a WAV File.
            if (data.contentType == "audio/wav")
            {
                var audioUrl = data.url;
                string blobName = audioUrl.Split('/').Last();

                string contosoStorageConnectionString = System.Environment.GetEnvironmentVariable("ContosoStorageConnectionString", EnvironmentVariableTarget.Process);
                string speechRegion = System.Environment.GetEnvironmentVariable("SpeechRegion", EnvironmentVariableTarget.Process);
                string speechKey = System.Environment.GetEnvironmentVariable("SpeechKey", EnvironmentVariableTarget.Process);
                string translatorKey = System.Environment.GetEnvironmentVariable("TranslatorKey", EnvironmentVariableTarget.Process);
                string translatorEndpoint = System.Environment.GetEnvironmentVariable("TranslatorEndpoint", EnvironmentVariableTarget.Process);
                string translatorLocation = System.Environment.GetEnvironmentVariable("TranslatorLocation", EnvironmentVariableTarget.Process);
                string cosmosEndpointUrl = System.Environment.GetEnvironmentVariable("CosmosDBEndpointUrl", EnvironmentVariableTarget.Process);
                string cosmosPrimaryKey = System.Environment.GetEnvironmentVariable("CosmosDBPrimaryKey", EnvironmentVariableTarget.Process);
                string textAnalyticsKey = System.Environment.GetEnvironmentVariable("TextAnalyticsKey", EnvironmentVariableTarget.Process);
                string textAnalyticsEndpoint = System.Environment.GetEnvironmentVariable("TextAnalyticsEndpoint", EnvironmentVariableTarget.Process);

                // Download audio file to a local temp directory
                var tempPath = System.IO.Path.GetTempFileName();
                BlobContainerClient container = new BlobContainerClient(contosoStorageConnectionString, "audiorecordings");
                BlobClient blob = container.GetBlobClient(blobName);
                await blob.DownloadToAsync(tempPath);

                var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                speechConfig.SetProperty(PropertyId.SpeechServiceConnection_SingleLanguageIdPriority, "Latency");

                // Audio Language Identification
                // Considering only two languages: English and Spanish
                // Languages supported for language detection : https://docs.microsoft.com/azure/cognitive-services/speech-service/language-support
                var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "en-US", "es-MX" });
                string languageDetected = "en-US";
                using (var audioInput = AudioConfig.FromWavFileInput(tempPath))
                {
                    using (var recognizer = new SourceLanguageRecognizer(speechConfig, autoDetectSourceLanguageConfig, audioInput))
                    {
                        var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                        if (result.Reason == ResultReason.RecognizedSpeech)
                        {
                            var lidResult = AutoDetectSourceLanguageResult.FromResult(result);
                            languageDetected = lidResult.Language;
                        }
                    }
                }
                speechConfig.SpeechRecognitionLanguage = languageDetected;

                // Audio Transcription
                StringBuilder sb = new StringBuilder();
                using var audioConfig = AudioConfig.FromWavFileInput(tempPath);
                {
                    using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
                    {
                        var stopRecognition = new TaskCompletionSource<int>();
                        recognizer.SessionStopped += (s, e) =>
                        {
                            stopRecognition.TrySetResult(0);
                        };
                        recognizer.Canceled += (s, e) =>
                        {
                            stopRecognition.TrySetResult(0);
                        };
                        recognizer.Recognized += (s, e) =>
                        {
                            if (e.Result.Reason == ResultReason.RecognizedSpeech)
                            {
                                sb.Append(e.Result.Text);
                            }
                            else if (e.Result.Reason == ResultReason.NoMatch)
                            {
                                log.LogInformation($"NOMATCH: Speech could not be recognized.");
                            }
                        };
                        await recognizer.StartContinuousRecognitionAsync();
                        Task.WaitAny(new[] { stopRecognition.Task });
                    }
                }
                string transcribedText = sb.ToString();

                // If transcription is in Spanish we will translate it to English
                if (!languageDetected.Contains("en"))
                {
                    string route = $"/translate?api-version=3.0&to=en";
                    string textToTranslate = sb.ToString();
                    object[] body = new object[] { new { Text = textToTranslate } };
                    var requestBody = JsonConvert.SerializeObject(body);

                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(translatorEndpoint + route);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", translatorKey);
                        request.Headers.Add("Ocp-Apim-Subscription-Region", translatorLocation);

                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                        var responseBody = await response.Content.ReadAsStringAsync();
                        List<Model.TranslatorService.Root> translatedDocuments = JsonConvert.DeserializeObject<List<Model.TranslatorService.Root>>(responseBody);
                        transcribedText = translatedDocuments.FirstOrDefault().Translations.FirstOrDefault().Text;
                    }
                }

                //Azure Text Analytics for Healthcare
                List<string> healthDocuments = new List<string>
                {
                    transcribedText
                };
                var textAnalyticsClient = new TextAnalyticsClient(new Uri(textAnalyticsEndpoint), new AzureKeyCredential(textAnalyticsKey));
                AnalyzeHealthcareEntitiesOperation healthOperation = textAnalyticsClient.StartAnalyzeHealthcareEntities(healthDocuments, "en", new AnalyzeHealthcareEntitiesOptions { });
                await healthOperation.WaitForCompletionAsync();
                AnalyzeHealthcareEntitiesResult healthcareResult = healthOperation.GetValues().FirstOrDefault().FirstOrDefault();
                
                //Insert documents into CosmosDB
                var cosmosClient = new CosmosClient(cosmosEndpointUrl, cosmosPrimaryKey);
                var cosmosDatabase = (await cosmosClient.CreateDatabaseIfNotExistsAsync("Contoso")).Database;
                var cosmosContainer = (await cosmosDatabase.CreateContainerIfNotExistsAsync("Transcriptions", "/id")).Container;

                Model.Transcription newTranscription = new Model.Transcription();
                newTranscription.Id = Guid.NewGuid().ToString();
                newTranscription.DocumentDate = new DateTime(int.Parse(blobName.Substring(0, 4)),
                    int.Parse(blobName.Substring(4, 2)), int.Parse(blobName.Substring(6, 2)));
                newTranscription.FileName = blobName;
                newTranscription.TranscribedText = transcribedText;
                foreach (var item in healthcareResult.Entities)
                {
                    newTranscription.HealthcareEntities.Add(new Model.HealthcareEntity() { Category = item.Category, Text = item.Text });
                }

                try
                {
                    ItemResponse<Model.Transcription> cosmosResponse = await
                        cosmosContainer.CreateItemAsync(newTranscription, new PartitionKey(newTranscription.Id));
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    //Conflicting documents are silently ignored for demo purposes.
                }

                System.IO.File.Delete(tempPath);
                log.LogInformation(eventGridEvent.Data.ToString());
            }
        }
    }
}
