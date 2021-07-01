// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentProcessing
{
    public static class ClaimsProcessing
    {
        [FunctionName("ClaimsProcessing")]
        public static async Task RunAsync([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            //Extracting content type and url of the blob triggering the function
            var jsondata = JsonConvert.SerializeObject(eventGridEvent.Data);
            var tmp = new { contentType = "", url = ""};
            var data = JsonConvert.DeserializeAnonymousType(jsondata, tmp);

            //Checking if the trigger was iniatiated for a PDF File.
            if (data.contentType == "application/pdf")
            {
                var pdfUrl = data.url;

                string endpoint = System.Environment.GetEnvironmentVariable("FormsRecognizerEndpoint", EnvironmentVariableTarget.Process);
                string apiKey = System.Environment.GetEnvironmentVariable("FormsRecognizerKey", EnvironmentVariableTarget.Process); 
                string contosoStorageConnectionString = System.Environment.GetEnvironmentVariable("ContosoStorageConnectionString", EnvironmentVariableTarget.Process);
                string cosmosEndpointUrl = System.Environment.GetEnvironmentVariable("CosmosDBEndpointUrl", EnvironmentVariableTarget.Process);
                string cosmosPrimaryKey = System.Environment.GetEnvironmentVariable("CosmosDBPrimaryKey", EnvironmentVariableTarget.Process);

                //Create a SAS Link to give Forms Recognizer read access to the document
                BlobServiceClient blobServiceClient = new BlobServiceClient(contosoStorageConnectionString);
                BlobContainerClient container = new BlobContainerClient(contosoStorageConnectionString, "claims");
                string blobName = pdfUrl.Split('/').Last();
                BlobClient blob = container.GetBlobClient(blobName);
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

                //TODO:Run the document through the model

                //Insert documents into CosmosDB
                var cosmosClient = new CosmosClient(cosmosEndpointUrl, cosmosPrimaryKey);
                var cosmosDatabase = (await cosmosClient.CreateDatabaseIfNotExistsAsync("Contoso")).Database;
                var cosmosContainer = (await cosmosDatabase.CreateContainerIfNotExistsAsync("Claims", "/InsuredID")).Container;

                Model.ClaimsDocument processedDocument = new Model.ClaimsDocument();
                processedDocument.DocumentDate = new DateTime(int.Parse(blobName.Substring(0, 4)), 
                    int.Parse(blobName.Substring(4, 2)), int.Parse(blobName.Substring(6, 2)));
                processedDocument.PatientName = forms[0].Fields["PatientName"].ValueData?.Text;
                processedDocument.InsuredID = forms[0].Fields["InsuredID"].ValueData?.Text;
                processedDocument.Diagnosis = forms[0].Fields["Diagnosis"].ValueData?.Text;
                decimal.TryParse(forms[0].Fields["TotalCharges"].ValueData?.Text, out decimal totalCharges);
                processedDocument.TotalCharges = totalCharges;                
                DateTime.TryParse(forms[0].Fields["PatientBirthDate"].ValueData?.Text, out DateTime patientBirthDate);
                processedDocument.PatientBirthDate = patientBirthDate;
                decimal.TryParse(forms[0].Fields["AmountPaid"].ValueData?.Text, out decimal amountPaid);
                processedDocument.AmountPaid = amountPaid;
                decimal.TryParse(forms[0].Fields["AmountDue"].ValueData?.Text, out decimal amountDue);
                processedDocument.AmountDue = amountDue;
                processedDocument.FileName = blobName;
                if (processedDocument.InsuredID == null)
                {
                    processedDocument.Id = Guid.NewGuid().ToString();
                }
                else
                {
                    processedDocument.Id = processedDocument.InsuredID;
                }                

                try
                {
                    ItemResponse<Model.ClaimsDocument> cosmosResponse = await
                        cosmosContainer.CreateItemAsync(processedDocument, new PartitionKey(processedDocument.InsuredID));
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    //Conflicting EnsurerID is silently ignored for demo purposes.
                }
            }
            log.LogInformation(eventGridEvent.Data.ToString());
        }
    }
}
