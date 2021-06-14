// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Training;
using Azure.AI.FormRecognizer.Models;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Specialized;

namespace DocumentProcessing
{
    public static class Function1
    {
        [FunctionName("ClaimsProcessing")]
        public static async Task RunAsync([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var jsondata = JsonConvert.SerializeObject(eventGridEvent.Data);
            var tmp = new { contentType = "", url = ""};
            var data = JsonConvert.DeserializeAnonymousType(jsondata, tmp);

            if (data.contentType == "application/pdf")
            {
                var pdfUrl = data.url;

                string endpoint = System.Environment.GetEnvironmentVariable("FormsRecognizerEndpoint", EnvironmentVariableTarget.Process);
                string apiKey = System.Environment.GetEnvironmentVariable("FormsRecognizerKey", EnvironmentVariableTarget.Process); 
                string claimsDocumentStorageConnectionString = System.Environment.GetEnvironmentVariable("ClaimsDocumentStorageConnectionString", EnvironmentVariableTarget.Process);

                //Create a SAS Link to give Forms Recognizer read access to the document
                BlobServiceClient blobServiceClient = new BlobServiceClient(claimsDocumentStorageConnectionString);
                BlobContainerClient container = new BlobContainerClient(claimsDocumentStorageConnectionString, "claims");
                BlobClient blob = container.GetBlobClient(pdfUrl.Split('/').Last());
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blob.GetParentBlobContainerClient().Name,
                    BlobName = blob.Name,
                    Resource = "b"
                };
                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                sasBuilder.SetPermissions(BlobSasPermissions.Read);
                Uri sasUri = blob.GenerateSasUri(sasBuilder);

                var credential = new AzureKeyCredential(apiKey);

                //Get the latest trained model
                var formTrainingClient = new FormTrainingClient(new Uri(endpoint), credential);
                Pageable<CustomFormModelInfo> formsModels = formTrainingClient.GetCustomModels();                
                var latestModel = (from inc in formsModels orderby inc.TrainingCompletedOn descending select inc).FirstOrDefault();

                //Run the document through the model
                var formRecognizerClient = new FormRecognizerClient(new Uri(endpoint), credential);
                var options = new RecognizeCustomFormsOptions() { IncludeFieldElements = true };
                RecognizeCustomFormsOperation operation = formRecognizerClient.StartRecognizeCustomFormsFromUri(latestModel.ModelId, sasUri, options);
                Response<RecognizedFormCollection> operationResponse = await operation.WaitForCompletionAsync();
                RecognizedFormCollection forms = operationResponse.Value;
            }
            log.LogInformation(eventGridEvent.Data.ToString());
        }
    }
}
