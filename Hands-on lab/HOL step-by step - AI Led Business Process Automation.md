![Microsoft Cloud Workshops](https://raw.githubusercontent.com/Microsoft/MCW-Template-Cloud-Workshop/main/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
AI Led Business Process Automation
</div>

<div class="MCWHeader2">
Hands-on lab step-by-step
</div>

<div class="MCWHeader3">
June 2021
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2021 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [AI Led Business Process Automation hands-on lab step-by-step](#ai-led-business-process-automation-hands-on-lab-step-by-step)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Overview](#overview)
  - [Solution architecture](#solution-architecture)
  - [Requirements](#requirements)
  - [Before the hands-on lab](#before-the-hands-on-lab)
  - [Exercise 1: Extract text and structure from documents with Forms Recognizer](#exercise-1-extract-text-and-structure-from-documents-with-forms-recognizer)
    - [Task 1: Prepare custom model to process documents](#task-1-prepare-custom-model-to-process-documents)
    - [Task 2: Configuring Azure Functions and Event Grid for document uploads](#task-2-configuring-azure-functions-and-event-grid-for-document-uploads)
    - [Task 3: Connecting CosmosDB and Forms Recognizer to Azure Functions](#task-3-connecting-cosmosdb-and-forms-recognizer-to-azure-functions)
    - [Task 4: Running document processing automation](#task-4-running-document-processing-automation)
  - [Exercise 2: Extract Health Analytics from visit audio records](#exercise-2-extract-health-analytics-from-visit-audio-records)
    - [Task 1: Configuring Azure Functions and Event Grid for audio uploads](#task-1-configuring-azure-functions-and-event-grid-for-audio-uploads)
    - [Task 2: Connecting Cognitive Services to Azure Functions](#task-2-connecting-cognitive-services-to-azure-functions)
    - [Task 3: Running audio record processing automation](#task-3-running-audio-record-processing-automation)
  - [Exercise 3: Using Azure Cognitive Search to index and serve data](#exercise-3-using-azure-cognitive-search-to-index-and-serve-data)
    - [Task 1: Setting up indexer for forms documents](#task-1-setting-up-indexer-for-forms-documents)
    - [Task 2: Setting up indexer for audio transcriptions and health analytics](#task-2-setting-up-indexer-for-audio-transcriptions-and-health-analytics)
    - [Task 3: Configuring the hospital portal](#task-3-configuring-the-hospital-portal)
  - [Exercise 4: Building custom PowerBI reports on healthcare data](#exercise-4-building-custom-powerbi-reports-on-healthcare-data)
    - [Task 1: Connecting PowerBI to CosmosDB](#task-1-connecting-powerbi-to-cosmosdb)
    - [Task 2: Setting up data transformations for semi-structured data](#task-2-setting-up-data-transformations-for-semi-structured-data)
    - [Task 3: Creating a custom PowerBI report from multiple data sets](#task-3-creating-a-custom-powerbi-report-from-multiple-data-sets)
  - [After the hands-on lab](#after-the-hands-on-lab)
    - [Task 1: Delete Azure resource groups](#task-1-delete-azure-resource-groups)

<!-- /TOC -->

# AI Led Business Process Automation hands-on lab step-by-step

## Abstract and learning objectives

\[Insert what is trying to be solved for by using this workshop. . . \]

## Overview

\[insert your custom workshop content here . . . \]

## Solution architecture

Below is a high-level architecture diagram of the solution you implement in this hands-on lab. Please review this carefully to understand the whole of the solution as you are working on the various components.

![This solution diagram includes a high-level overview of the architecture implemented within this hands-on lab.](media/architecture-diagram.png "Solution architecture diagram")

> **Note:** The solution provided is only one of many possible, viable approaches.

Hospitals in the Contoso Healthcare network provide PDF files of claim forms and WAV files of visit audio recordings via blobs in an Azure Storage account. Two event grid subscriptions propagate the blob creation events that trigger two separate functions in a [Function App](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-function-app-portal).

One of the functions handles PDF processing. The function uses an [Azure Forms Recognizer](https://azure.microsoft.com/en-us/services/form-recognizer/) that has a custom trainer model to extract the required information from forms. Once the metadata is extracted, the function saves the result in [Azure CosmosDB](https://azure.microsoft.com/en-us/services/cosmos-db/), allowing Contoso to build custom PowerBI reports with a direct query connection. Additionally, the data is indexed in an [Azure Cognitive Search](https://azure.microsoft.com/en-us/services/search/) to be served in a unified search experience on the internal hospital portal.

A second function in the Function App processes audio recordings. Contoso uses [Azure Cognitive Speech Audio Language Identification](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-automatic-language-detection?pivots=programming-language-csharp) to detect the language of the audio file and transcribe it to text. Once the text transcriptions are ready, Spanish transcriptions are translated to English using [Azure Cognitive Services Text Translator](https://azure.microsoft.com/en-us/services/cognitive-services/translator/). Finally, [Azure Cognitive Services Text Analytics for Health](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/how-tos/text-analytics-for-health?tabs=ner) is used to extract and label relevant medical information to provide a richer search experience in the internal hospital portal. Once the results are ready, the function saves the data in an Azure CosmosDB collection to be indexed by Azure Cognitive Search.

Finally, the internal hospital portal queries the indexes created in Azure Cognitive Search, offering a unified search experience for both structured and unstructured data sets.

## Requirements

- Microsoft Azure subscription must be pay-as-you-go or MSDN.
  - Trial subscriptions will _not_ work.

## Before the hands-on lab

Refer to the Before the hands-on lab setup guide manual before continuing to the lab exercises.

## Exercise 1: Extract text and structure from documents with Forms Recognizer

Duration: 45 minutes

Azure Form Recognizer is a part of [Azure Applied AI Services](https://docs.microsoft.com/en-us/azure/applied-ai-services/) that lets you build automated data processing software using machine learning technology. You can identify and extract text, key/value pairs, selection marks, tables, and structure from your documents. The service outputs structured data that includes the relationships in the original file, bounding boxes, confidence, and more. You can quickly get accurate results tailored to your specific content without heavy manual intervention or extensive data science expertise. Form Recognizer comprises custom document processing models, prebuilt models for invoices, receipts, IDs and business cards, and the layout model.

Contoso has its own document template for claims processing. In this exercise, you will be using a set of documents to train a custom model with labels. When you train with labeled data, the model uses supervised learning to extract values of interest, using the labeled forms you provide. In this case, Form Recognizer uses the [Layout API](https://docs.microsoft.com/en-us/azure/cognitive-services/form-recognizer/concept-layout) to learn the expected sizes and positions of printed and handwritten text elements and extract values.

### Task 1: Prepare custom model to process documents

1. To access the training data in Azure Storage, we need a [SAS](https://docs.microsoft.com/en-us/rest/api/storageservices/delegate-access-with-shared-access-signature) access link. In the [Azure portal](https://portal.azure.com), navigate to your **contosoSUFFIX** Storage Account Overview page by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contosoSUFFIX** Storage Account from the list of resources.

   ![Lab resource group is open. The storage account is highlighted.](media/select-storage-account.png "Storage Account Selection")

2. Once you are on the Storage Account Overview page, switch to the **Containers (1)** panel.  Select **claimstraining (2)** container and open the context menu with the three dots **(3)** at the end of the row. Select **Generate SAS (4)**.

   ![Claimstraining container is selected. Generate SAS command from the context menu is highlighted.](media/storage-generate-sas-claimstraining.png "Generate SAS for Container")

3. On the Generate SAS panel, select all **Permissions (1)**. Set the **Expiry (2)** date to a future date so that the link does not expire while you work on your lab. Select **Generate SAS token and URL (3)** and copy the **Blob SAS URL (4)** in a text editor of your choice to be used in upcoming steps during the lab.

   ![Generate SAS panel is open. All permissions are selected. The expiry date is set to a month further. Generate SAS token, and URL button is selected. Copy button for SAS URL is highlighted.](media/storage-generate-sas-claimstraining-copy.png "Blob SAS URL Copy")

4. Go back to your resource group and select **contoso-fr-SUFFIX (1)** Azure Forms Recognizer service where SUFFIX represents a unique string specific to your resource group.

   ![Lab resource group is open. Azure Forms Recognizer account is highlighted.](media/select-forms-recognizer.png "Select Forms Recognizer Service")

5. Switch to the **Keys and Endpoint (1)** panel. Copy **Key 1 (2)** and **Endpoint (3)** to a text editor of your choice to be used in upcoming steps during the lab.

   ![Forms Recognizer Keys and Endpoint panel is shown. Key 1 Copy and Endpoint Copy buttons are highlighted.](media/get-forms-recognizer-keys.png "Copy Forms Recognizer Key and Endpoint")

6. Navigate to <https://fott-2-1.azurewebsites.net/> in a browser window.

7. Select **Use Custom to train a model with labels and get key value pairs**.

   ![Welcome screen is shown. Custom model training option is highlighted.](media/fott-custom-model.png "Custom Train Model")

8. Select **New Project**.

   ![New Custom Training Project selection is highlighted.](media/fott-new-project.png "New Project")

9. Set **Display Name (1)** to **ContosoDocuments** and select **Add Connection (2)**.

   ![Project Settings page is open. Display name is set to ContosoDocuments. Add connection button is highlighted.](media/fott-new-connection.png "Add Source Connection")

10. Set **Display name (1)** to **DocumentSource** and past the previously copies SAS URL into the **SAS URI (2)** box. Select **Save Connection (3)** to continue.

    ![Blob Connection Settings page is open. Display name is set to DocumentSource. SAS URI is set. Save connection button is highlighted.](media/fott-storage-sas.png "Setting SAS URL")

11. Select **Go Back (1)** from your browser twice to go back to the **Project Settings** page.

12. Set the values listed below.

    - **Source Connection (2):** **DocumentSource**
    - **Form recognizer service URI (3):** Previously copied **Endpoint** value from Forms Recognizer.
    - **API key (4):** Previously copied **Key 1** value from Forms Recognizer.

    ![Source connection is set to DocumentSource. Forms recognizer service URI and API Key are copied from previously captured values. Save Project button is highlighted.](media/fott-project-settings.png "Setting Project Settings")

    Select **Save Project (5)** to continue.

13. The documents you will use for training can be seen in the **documents panel (1)**. Observe the **tags (2)** assigned for various fields in the documents and how values are extracted with the Layout API.

    ![Tags Editor is shown. Documents and Tags Lists are highlighted. PatientName, PatientBirthDate, InsuredID, AmountDue, AmountPaid, TotalCharges, and Diagnosis tags are created, and matching values are extracted from the document.](media/fott-labelled-documents.png "Tag Editor")

14. Switch to the **Train (1)** page. Set **Model name (2)** to **ContosoModel** and select **Train (3)** to start the model training.

    ![Model training page is open. The model name is set to ContosoModel. Train button is highlighted.](media/fott-train-model.png "Model Training")

15. Your trained model is ready. Observe the estimated accuracy for each tag/label **(1)** and average accuracy **(2)** for the model.  

    ![Model training result page is shown. Estimated Accuracy values and Average Accuracy values are presented. Average accuracy is 97.60%.](media/fott-model-trained.png)

### Task 2: Configuring Azure Functions and Event Grid for document uploads

As part of its automation process, Contoso will upload claims documents in the form of PDF files to an Azure Storage account as blobs. An Azure Function App has to detect new files and process them with the trained Forms Recognizer Model. [Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/overview) is the perfect candidate to build applications with event-based architectures thanks to its built-in support for events coming from Azure services, like storage blobs and resource groups. For the Functions App to detect new blobs, you will be using an Azure Event Grid subscription and defining an event handler for the matching Azure Function.

1. In the [Azure portal](https://portal.azure.com), navigate to your **contosoSUFFIX** Storage Account Overview page by selecting **Resource groups** from Azure services list selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contosoSUFFIX** Storage Account from the list of resources.

   ![Lab resource group is open. The storage account is highlighted.](media/select-storage-account.png "Storage Account Selection")

2. Switch to the **Events (1)** panel. Make sure you are on the **Get Started (2)** page. Select **Azure Function (3)** as the event destination type. Select **Create (4)** to continue.

   ![Storage account page is open. The events panel is shown. Azure Functions is selected. Create button is highlighted.](media/storage-event-function.png "Create Storage Event Subscription")

3. From the list of function apps, select the arrow **(1)** for the function app named **contoso-func-SUFFIX** to get a list of functions available. From the list, select the **ClaimsProcessing** function.

   ![Function Apps are listed. Contoso function app functions are shown. ClaimsProcessing function is highlighted.](media/event-grid-select-claimsprocessing.png "Function Selection for Event Grid")

4. Select **Add Event Grid Subscription (1)**.

   ![ClaimsProcessing function is selected. Add Event Grid Subscription link is highlighted.](media/event-grid-add-subscription.png "Add Event Grid Subscription")

5. Set the values listed below.

    - **Name (1):** **DocumentEvents**
    - **Topic Type (2):** Storage account.
    - **Source Resource (3):** Contoso storage account.
    - **System Topic Name (4):** **DocumentEvent**
    - **Filter to Event Types (5):** Blob Created

   ![Create event subscription page is presented. The event name is set to DocumentEvents. Topic Type is set to Storage account. Source Resource is set to contosoSUFFIX storage account. System Topic Name is set to DocumentEvent. Blob Created and Blob Deleted events are selected. Create button is highlighted.](media/event-grid-create-subscription.png "Event Grid Subscription Settings")

    Select **Create (6)** to continue.

### Task 3: Connecting CosmosDB and Forms Recognizer to Azure Functions

For the document processing automation, our Azure Function must read the documents from Azure Storage, connect to Azure Forms Recognizer and use the trained model, and finally connect to CosmosDB to save the final results. In this task, we will connect all the required services to the ClaimsProcessing function.

1. In the [Azure portal](https://portal.azure.com), navigate to your **contosoSUFFIX** Storage Account Overview page by selecting **Resource groups** from Azure services list selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contosoSUFFIX** Storage Account from the list of resources.

   ![Lab resource group is open. The storage account is highlighted.](media/select-storage-account.png "Storage Account Selection")

2. Switch to the **Access keys (1)** panel. Select **Show keys (2)** to reveal the keys. Select the copy button **(3)** for the first connection string and paste it to a text editor of your choice to be used in the following steps.

   ![Storage Account Access keys page is shown. The show Keys button is selected. The copy button for the first connection string is highlighted.](media/get-storage-connection-string.png "Copy Storage Connection String")

3. Go back to your resource group and find your Cosmos DB account in your lab resource group. Select it to navigate to its Overview page.

   ![Resource group page is open. CosmosDB service is highlighted.](media/select-cosmosdb-service.png "Select Cosmos DB service.")

4. Switch to the **Keys (1)** panel. Copy the values for **URI (2)** and **PRIMARY KEY (3)** to a text editor of your choice to be used in the following steps.

   ![Keys panel of the Cosmos DB account is open. The copy buttons for URI and Primary Key are highlighted.](media/get-cosmosdb-keys.png "Cosmos DB Key and URI")

5. Go back to your resource group and find your Function App named **contoso-func-SUFFIX** in your lab resource group. Select it to navigate to its Overview page.

   ![Resource group page is open. Function App is highlighted.](media/select-azure-function.png "Select Function App.")

6. Switch to the **Configuration (1)** panel. Select **New application setting (2)**.

   ![Function App Configuration page is open. New application setting link is highlighted.](media/function-app-new-application-setting.png "Function App New Application Setting")

7. Set **Name (1)** to **ContosoStorageConnectionString** and **Value (2)** to the previously copied Contoso storage account connection string. Select **OK (3)** to save.

   ![Add Edit Application setting panel is open. Name is set to ContosoStorageConnectionString. Value is set to the previously copied Contoso storage account connection string. OK button is highlighted.](media/function-app-setting-contoso-storage.png)

8. Repeat the same steps to add the **Application Settings** listed below.

   | Name                    | Value                                               |
   |-------------------------|-----------------------------------------------------|
   | FormsRecognizerEndpoint | Previously copied **Endpoint** for Forms Recognizer |
   | FormsRecognizerKey      | Previously copied **Key 1** for Forms Recognizer    |
   | CosmosDBEndpointUrl     | Previously copied **URI** for Cosmos DB             |
   | CosmosDBPrimaryKey      | Previously copied **Primary Key** for Cosmos DB     |

9. Once all settings **(1)** are set, select **Save (2)**.

   ![New application settings are highlighted. Save button is pointed.](media/function-app-settings-save.png "Save new application settings")

### Task 4: Running document processing automation

Now that all implementations are completed, we can upload a new document to the storage and see the entire process extracting values from claims submissions.

1. In the [Azure portal](https://portal.azure.com), navigate to your **LabVM** Virtual Machine by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **WabVM** Virtual Machine from the list of resources.

    ![The WebVM virtual machine is highlighted in the list of resources.](media/select-labvm.png "WebVM Selection")

2. On the LabVM Virtual Machine's **Overview** blade, select **Connect (1)** and **RDP (2)** on the top menu.

   ![The LabVM VM blade is displayed, with the Connect button highlighted in the top menu.](media/connect-rdp-labvm.png "LabVM RDP Connect")

3. Select **Download RDP File** on the next page, and open the downloaded file.

    > **Note**: The first time you connect to the LabVM Virtual Machine, you will see a blue pop-up terminal dialog taking you through a couple of software installs. Don't be alarmed, and wait until the installs are complete.

    ![RDP Window is open. Download RDP File button is highlighted.](media/rdp-download.png "LabVM RDP File Download")

4. Select **Connect** on the Remote Desktop Connection dialog.

    ![In the Remote Desktop Connection Dialog Box, the Connect button is highlighted.](media/remote-desktop-connection-labvm.png "Remote Desktop Connection dialog")

5. Enter the following credentials with your password when prompted, and then select **OK**:

   - **Username**: demo
   - **Password**: {YOUR-ADMIN-PASSWORD}
  
    > **Note**: default password is `Password.1!!`

    ![The credentials specified above are entered into the Enter your credentials dialog.](media/rdp-credentials-labvm.png "Enter your credentials")

6. Select **Yes** to connect if prompted that the remote computer's identity cannot be verified.

    ![In the Remote Desktop Connection dialog box, a warning states that the remote computer's identity cannot be verified and asks if you want to continue anyway. At the bottom, the Yes button is circled.](media/remote-desktop-connection-identity-verification-labvm.png "Remote Desktop Connection dialog")

7. Once logged into the LabVM VM, a script will execute to install the various items needed for the remaining lab steps.

8. Once the script completes, open **Edge** and navigate to the [Azure portal](https://portal.azure.com). Enter your credentials to access your subscriptions. Navigate to **contosoSUFFIX** storage account by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contosoSUFFIX** Storage Account from the list of resources.

    ![Edge is highlighted on the desktop. Browser is open and navigated to portal.azure.com. Storage account overview page is open.](media/azure-portal-labvm.png "Storage Account on Lab VM")

9. Switch to the **Containers (1)** panel. Select the **claims (2)** container.

   ![Contoso storage containers are listed. Claims container is highlighted.](media/storage-claims-container.png "Claims Storage Container")

10. Select **Upload (1)** and **Browse (2)**. Navigate to `C:\MCW\MCW-main\Hands-on lab\lab-files\claims-forms` **(3)**. Pick **20210621-test-form (4)** and select **Open (5)**. This PDF file is brand new in the eyes of our trained model and not used during model training.

    ![Container page is open. The upload button is selected. File open dialog shows the claims-forms folder with PDF files listed. 20210621-test-form PDF file and Open button are highlighted.](media/upload-test-claims-form.png "Local file selection for upload.")

11. Select **Upload** to start the upload process.

    ![Upload blob dialog is open. 20210621-test-form.pdf is selected. Upload button is highlighted.](media/storage-upload-claims-form.png "File Upload")

12. Now, if everything went smoothly, we should see the result in the Cosmos DB service. Go back to your resource group and find your Cosmos DB Account named `contoso-cdb-SUFFIX` in your lab resource group. Select it to navigate to its Overview page.

    ![Resource group page is open. CosmosDB service is highlighted.](media/select-cosmosdb-service.png "Select Cosmos DB service.")

13. Select **Data Explorer**.

    ![Cosmos DB Overview page is open. Data explorer button is highlighted.](media/cosmosdb-data-explorer.png "Cosmos DB Data Explorer")

14. Select the **Items (1)** list under the **Contoso** database's **Claims** collection. Select the first document **(2)** to see its content. Take a look at the values extracted by Forms Recognizer, such as **PatientName** and **Diagnosis (3)**.

    ![Cosmos DB Data Explorer is open. Claims Document values are shown as a document in Claims collection in the Contoso database.](media/cosmosdb-data-explorer-claims-document.png "Claims Document in Cosmos DB")

15. To have some more data in the Cosmos DB Claims collection, go back and upload all the PDF files available in the `C:\MCW\MCW-main\Hands-on lab\lab-files\claims-forms` folder.

    ![All PDF files are selected. Open button is highlighted.](media/storage-upload-claims-forms.png "File Upload")

    You can go back to CosmosDB Claims collection and observe the new results.

## Exercise 2: Extract Health Analytics from visit audio records

Duration: 45 minutes

Contoso uploads audio recordings of patient visits to an Azure Storage Blob service. An Azure Function will be triggered with an Event Grid subscription/event handler to process recordings. The function will first detect the language of the recording using [Azure Cognitive Speech Audio Language Identification](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/how-to-automatic-language-detection?pivots=programming-language-csharp) and then transcribe it to text. Once transcriptions are ready, Spanish records will be translated to English based on Contoso's requirements. Finally, [Azure Cognitive Services Text Analytics for Health](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/how-tos/text-analytics-for-health?tabs=ner) will extract and label relevant medical information to provide a richer search experience. During the exercise, you will integrate all the pieces, run a couple of sample recordings, and observe the results.

### Task 1: Configuring Azure Functions and Event Grid for audio uploads

As part of its automation process, Contoso will upload audio recordings of patient visits as WAV files to an Azure Storage account as blobs. An Azure Function App has to detect new files and process them with multiple Cognitive Services. For the Functions App to detect new blobs, you will be using a new Azure Event Grid subscription and defining an event handler for the matching Azure Function.

1. In the [Azure portal](https://portal.azure.com), navigate to your **contosoSUFFIX** Storage Account Overview page by selecting **Resource groups** from Azure services list selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contosoSUFFIX** Storage Account from the list of resources.

   ![Lab resource group is open. The storage account is highlighted.](media/select-storage-account.png "Storage Account Selection")

2. Switch to the **Events (1)** panel. Make sure you are on the **Get Started (2)** page. Select **Azure Function (3)** as the event destination type. Select **Create (4)** to continue.

   ![Storage account page is open. The events panel is shown. Azure Functions is selected. Create button is highlighted.](media/storage-event-function.png "Create Storage Event Subscription")

3. From the list of function apps, select the arrow **(1)** for the function app named **contoso-func-SUFFIX** to get a list of functions available. From the list, select the **AudioProcessing** function.

   ![Function Apps are listed. Contoso function app functions are shown. ClaimsProcessing function is highlighted.](media/event-grid-select-audioprocessing.png "Function Selection for Event Grid")

4. Select **Add Event Grid Subscription (1)**.

   ![ClaimsProcessing function is selected. Add Event Grid Subscription link is highlighted.](media/event-grid-add-subscription-for-audioprocessing.png "Add Event Grid Subscription")

5. Set the values listed below.

    - **Name (1):** **AudioEvents**
    - **Topic Type (2):** Storage account.
    - **Source Resource (3):** Contoso storage account.
    - **System Topic Name (4):** **DocumentEvent**
    - **Filter to Event Types (5):** Blob Created

   ![Create event subscription page is presented. The event name is set to DocumentEvents. Topic Type is set to Storage account. Source Resource is set to contosoSUFFIX storage account. System Topic Name is set to DocumentEvent. Blob Created and Blob Deleted events are selected. Create button is highlighted.](media/event-grid-create-subscription-for-audio.png "Event Grid Subscription Settings")

    Select **Create (6)** to continue.

### Task 2: Connecting Cognitive Services to Azure Functions

For audio recording processing, the AudioProcessing function will use multiple Cognitive Service accounts. Cognitive Services Speech will be used to detect the language of the recording and to transcribe the audio file. Cognitive Services Translator will be used to translate Spanish transcriptions to English. Finally, Cognitive Services Text Analytics will be used to extract and label relevant medical information from transcriptions. In this task, you will be connecting all the required Cognitive Services to the AudioProcessing Azure Function.

1. In the [Azure portal](https://portal.azure.com), navigate to your **contoso-speech-SUFFIX** Cognitive Service Overview page by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contoso-speech-SUFFIX** Cognitive Service Account from the list of resources.

   ![Lab resource group is open. The Cognitive Service Speech account is highlighted.](media/select-speech-account.png "Cognitive Service Speech Account Selection")

2. Switch to the **Keys and Endpoint (1)** panel. Copy **Key 1 (2)** and **Location (3)** to a text editor of your choice to be used in upcoming steps during the lab.

   ![Cognitive Services Speech Keys and Endpoint panel is shown. Key 1 Copy and Location Copy buttons are highlighted.](media/get-speech-service-keys.png "Copy Cognitive Service Speech Key and Endpoint")

3. Go back to your resource group and find your Cognitive Service named **contoso-translate-SUFFIX** in your lab resource group. Select it to navigate to its Overview page.

   ![Lab resource group is open. The Cognitive Service Translate account is highlighted.](media/select-translate-account.png "Cognitive Service Translate Account Selection")

4. Switch to the **Keys and Endpoint (1)** panel. Copy **Key 1 (2)**, **Location (3)** and **Text Translation Endpoint (4)** to a text editor of your choice to be used in upcoming steps during the lab.

   ![Cognitive Services Translate Keys and Endpoint panel is shown. Key 1 Copy, Location Copy, and Text Translation copy buttons are highlighted.](media/get-translate-service-keys.png "Copy Cognitive Service Translate Key and Endpoint")

5. Go back to your resource group and find your Cognitive Service named **contoso-textanalytics-SUFFIX** in your lab resource group. Select it to navigate to its Overview page.

   ![Lab resource group is open. The Cognitive Service Text Analytics account is highlighted.](media/select-textanalytics-account.png "Cognitive Service Text Analytics Account Selection")

6. Switch to the **Keys and Endpoint (1)** panel. Copy **Key 1 (2)** and **Endpoint (3)** to a text editor of your choice to be used in upcoming steps during the lab.

   ![Cognitive Services Text Analytics Keys and Endpoint panel is shown. Key 1 Copy and Endpoint copy buttons are highlighted.](media/get-textanalytics-service-keys.png "Copy Cognitive Service Text Analytics Key and Endpoint")

7. Go back to your resource group and find your Function App named **contoso-func-SUFFIX** in your lab resource group. Select it to navigate to its Overview page.

   ![Resource group page is open. Function App is highlighted.](media/select-azure-function.png "Select Function App.")

8. Switch to the **Configuration (1)** panel. Select **New application setting (2)**.

   ![Function App Configuration page is open. New application setting link is highlighted.](media/function-app-new-application-setting-step2.png "Function App New Application Setting")

9. Set **Name (1)** to **SpeechRegion** and **Value (2)** to the previously copied Speech service's **Location**. Select **OK (3)** to save.

   ![Add Edit Application setting panel is open. Name is set to ContosoStorageConnectionString. Value is set to the previously copied Contoso storage account connection string. OK button is highlighted.](media/function-app-setting-speech-region.png)

10. Repeat the same steps to add the **Application Settings** listed below.

    | Name                  | Value                                                                                           |
    |-----------------------|-------------------------------------------------------------------------------------------------|
    | SpeechKey             | Previously copied **Key 1** for Cognitive Services Speech Account                               |
    | TranslatorKey         | Previously copied **Key 1** for Cognitive Services Text Translation Account                     |
    | TranslatorEndpoint    | Previously copied **Text Translation Endpoint** for Cognitive Services Text Translation Account |
    | TranslatorLocation    | Previously copied **Location** for Cognitive Services Text Translation Account                  |
    | TextAnalyticsKey      | Previously copied **Key 1** for Cognitive Services Text Analytics Account                       |
    | TextAnalyticsEndpoint | Previously copied **Endpoint** for Cognitive Services Text Analytics Account                    |

11. Once all settings **(1)** are set, select **Save (2)**.

    ![New application settings are highlighted. Save button is pointed.](media/function-app-settings-save-step2.png "Save new application settings")

### Task 3: Running audio record processing automation

Now that all implementations are completed, we can upload new patient recordings and see the entire process of transcription, translation, and the extraction of medical information from audio files.

1. Connect to your LABVM. Open **Edge** and navigate to the [Azure portal](https://portal.azure.com). Enter your credentials to access your subscriptions. Navigate to **contosoSUFFIX** storage account by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contosoSUFFIX** Storage Account from the list of resources.

    ![Edge is highlighted on the desktop. Browser is open and navigated to portal.azure.com. Storage account overview page is open.](media/azure-portal-labvm.png "Storage Account on Lab VM")

2. Switch to the **Containers (1)** panel. Select the **audiorecordings (2)** container.

   ![Contoso storage containers are listed. audiorecordings container is highlighted.](media/storage-audiorecordings-container.png "audiorecordings Storage Container")

3. Select **Upload (1)** and **Browse (2)**. Navigate to `C:\MCW\MCW-main\Hands-on lab\lab-files\audio-recordings` **(3)**. Choose all the files **(4)** and select **Open (5)**.

    ![Container page is open. The upload button is selected. File open dialog shows the audio-recordings folder with WAV files listed. All WAV files and Open button are highlighted.](media/upload-audio-recordings.png "Local file selection for upload.")

4. Select **Upload** to start the upload process.

5. Now, if everything went smoothly, we should see the result in the Cosmos DB service. Go back to your resource group and find your Cosmos DB Account named `contoso-cdb-SUFFIX` in your lab resource group. Select it to navigate to its Overview page.

    ![Resource group page is open. CosmosDB service is highlighted.](media/select-cosmosdb-service.png "Select Cosmos DB service.")

6. Select **Data Explorer**.

    ![Cosmos DB Overview page is open. Data explorer button is highlighted.](media/cosmosdb-data-explorer.png "Cosmos DB Data Explorer")

7. Select the **Items (1)** list under the **Contoso** database's **Transcriptions** collection. Select the first document **(2)** to see it's content. Take a look at the **TranscribedText (3)** and **HealthcareEntities (4)** extracted.

    ![Cosmos DB Data Explorer is open. Claims Document values are shown as a document in Claims collection in the Contoso database.](media/cosmosdb-data-explorer-audio-transcriptions.png "Claims Document in Cosmos DB")

## Exercise 3: Using Azure Cognitive Search to index and serve data

Duration: X minutes

Contoso has an internal web portal hosted in an Azure App Service where staff can access various content and forms. The organization is looking to enhance the portal by centralizing patient information, streamlining, unifying, and simplifying access to claims documents and audio recordings. In this exercise, you will be indexing claims and audio transcriptions data sets in a Cognitive Search service using indexers that connect to Cosmos DB. [Azure Cognitive Search](https://docs.microsoft.com/en-us/azure/search/search-what-is-azure-search) is a cloud search service that gives developers an architecture, APIs, and tools to build rich search experiences over private, heterogenous content in web, mobile, and enterprise applications. Finally, you will configure the web portal to use the indexes for a unified search experience enriched with health analytics metadata.

### Task 1: Setting up indexer for forms documents

1. In the [Azure portal](https://portal.azure.com), navigate to your **contoso-search-SUFFIX** Search service's Overview page by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contoso-search-SUFFIX** Search service from the list of resources.

   ![Lab resource group is open. The search service is highlighted.](media/select-search-service.png "Search Service Selection")

2. Once you are on the **Overview (1)** page, select **Import data (2)** to continue.

   ![Azure Cognitive Search Overview page is open. Import data is highlighted.](media/search-import-data.png "Import Data for Search")

3. On the **Connect to your data** step set **Data Source (1)** to **Azure Cosmos DB** and **Data source name (2)** to **claimssource**. Select **Choose an existing connection (3)** to continue.

   ![Connect to your data page is open. The data source is set to Azure Cosmos DB. The data source name is set to claimssource. Choose an existing connection link is highlighted.](media/search-import-data-claims.png "Connecting to Cosmos DB")

4. Select **contoso-cdb-SUFFIX** Cosmos DB account as the source.

   ![Azure Cosmos DB account selection screen is open. contoso-cdb-SUFFIX is highlighted.](media/search-import-data-pick-cosmosdb.png "Cosmos DB Account Selection")

5. Set **Database (1)** to **Contoso** and **Collection (2)** to **Claims**. Select **Next: Add cognitive skills (Optional)** to Continue.

   ![Connect to your data page is open. The database is set to Contoso. The collection is set to Claims. Next: Add cognitive skills (Optional) button is highlighted.](media/search-import-data-claims-2.png "Connect to Cosmos DB Data")

6. On the **Add cognitive skills (Optional)** page, select **Skip to: Customize target Index**.

7. On the **Customize target index** page set **Index name (1)** to **claims-index**. Make sure Retrievable, Filterable, Sortable, Facetable, and Searchable checkboxes **(2)** for all fields match the setup in the following screenshot. Select **Next: Create an indexer** to continue.

   ![Customize target index page is open. The index name is set to claims-index. Retrievable is enabled for all fields. Filterable is enabled for all fields except id and rid. Sortable is enabled for all fields except id, PatientName, DocumentDate, FileName, and rid. Facetable is enabled for PatientBirthDate and Diagnosis. Searchable is enabled for PatientName, InsuredID, Diagnosis and FileName. Next: Create an indexer button is highlighted.](media/search-claims-index-setup.png "Customize Target Index")

8. On the **Create an indexer** page set **Name (1)** to **claims-indexer** and select **Submit (2)** to continue. We will leave the indexer schedule set to **Once** as we are not expecting data changes in Cosmos DB in our lab environment. However, you should set a different schedule to index new documents saved in Cosmos DB.

   ![Create an indexer page is open. The name is set to claims-indexer. Submit button is highlighted.](media/search-claims-indexer-schedule.png "Submit Indexer Job")

9. Once you are back on the **Overview (1)** page, switch to the **Indexers (2)** list to see the status **(3)** of the indexer and the number of documents indexed **(4)**. Select **Search explorer (5)** to continue.

   ![Search service's Overview page is open. Indexers list is shown. The success status of the indexer is highlighted. Processed documents count is highlighted.](media/search-claims-indexed.png "Indexer Status")

10. Select **Search (1)** to see a list of documents **(2)** from the index.

    ![Search explorer is open. The search button is selected. The search result is highlighted.](media/search-claims-result.png "Search Result")

### Task 2: Setting up indexer for audio transcriptions and health analytics

1. In the [Azure portal](https://portal.azure.com), navigate to your **contoso-search-SUFFIX** Search service's Overview page by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contoso-search-SUFFIX** Search service from the list of resources.

   ![Lab resource group is open. The search service is highlighted.](media/select-search-service.png "Search Service Selection")

2. Once you are on the **Overview (1)** page, select **Import data (2)** to continue.

   ![Azure Cognitive Search Overview page is open. Import data is highlighted.](media/search-import-data.png "Import Data for Search")

3. On the **Connect to your data** step set **Data Source (1)** to **Azure Cosmos DB** and **Data source name (2)** to **audiosource**. Select **Choose an existing connection (3)** to continue.

   ![Connect to your data page is open. The data source is set to Azure Cosmos DB. The data source name is set to audiosource. Choose an existing connection link is highlighted.](media/search-import-data-audio.png "Connecting to Cosmos DB")

4. Select **contoso-cdb-SUFFIX** Cosmos DB account as the source.

   ![Azure Cosmos DB account selection screen is open. contoso-cdb-SUFFIX is highlighted.](media/search-import-data-pick-cosmosdb.png "Cosmos DB Account Selection")

5. Set **Database (1)** to **Contoso** and **Collection (2)** to **Transcriptions**. Select **Next: Add cognitive skills (Optional)** to Continue.

   ![Connect to your data page is open. The database is set to Contoso. The collection is set to Transcriptions. Next: Add cognitive skills (Optional) button is highlighted.](media/search-import-data-audio-2.png "Connect to Cosmos DB Data")

6. On the **Add cognitive skills (Optional)** page, select **Skip to: Customize target Index**.

7. On the **Customize target index** page set **Index name (1)** to **audio-index**. Make sure Retrievable, Filterable, Sortable, Facetable, and Searchable checkboxes **(2)** for all fields match the setup in the following screenshot. Select **Next: Create an indexer** to continue.

   ![Customize target index page is open. The index name is set to claims-index. Retrievable is enabled for all fields except id. Filterable is enabled HealthcareEntities/Category and HealthcareEntities/Text. Sortable is disabled for all fields. Facetable is enabled for HealthcareEntities/Category and HealthcareEntities/Text. Searchable is enabled for  TranscribedText, FileName, HealthcareEntities/Category and HealthcareEntities/Text. Next: Create an indexer button is highlighted.](media/search-audio-index-setup.png "Customize Target Index")

8. On the **Create an indexer** page set **Name (1)** to **audio-indexer** and select **Submit (2)** to continue. We will leave the indexer schedule set to **Once** as we are not expecting data changes in Cosmos DB in our lab environment. However, you should set a different schedule to index new documents saved in Cosmos DB.

   ![Create an indexer page is open. The name is set to audio-indexer. Submit button is highlighted.](media/search-audio-indexer-schedule.png "Submit Indexer Job")

9. Once you are back on the **Overview (1)** page, switch to the **Indexers (2)** list to see the status **(3)** of the audio-indexer and the number of documents indexed **(4)**. Select **Search explorer (5)** to continue.

   ![Search service's Overview page is open. Indexers list is shown. The success status of the indexer is highlighted. Processed documents count is highlighted.](media/search-audio-indexed.png "Indexer Status")

10. Select **Search (1)** to see a list of documents having the **TranscribedText (2)** and **Healthcare Entities (3)** fields.

    ![Search explorer is open. The search button is selected. The search result is highlighted.](media/search-audio-result.png "Search Result")

### Task 3: Configuring the hospital portal

In this task, we will connect our Azure Cognitive Search indexes with the hospital portal and provide read access to the original documents so that the portal can show the actual files as well.

1. In the [Azure portal](https://portal.azure.com), navigate to your **contoso-search-SUFFIX** Search service's Overview page by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contoso-search-SUFFIX** Search service from the list of resources.

   ![Lab resource group is open. The search service is highlighted.](media/select-search-service.png "Search Service Selection")

2. On the **Overview (1)** page, copy the **Url (2)** for the service to a text editor of your choice to be used in upcoming steps during the lab.

   ![Overview page for the search service is open. URL Copy button is highlighted.](media/search-copy-endpoint.png "Endpoint URL for Azure Search")

3. Switch to the **Keys (1)** page and copy the **Primary admin key (2)** to a text editor of your choice to be used in upcoming steps during the lab.

   ![Keys page for the search service is open. Copy button for the primary admin key is highlighted.](media/search-copy-key.png "Search Service Primary Admin Key")

4. Go back to your resource group and select **contosoSUFFIX** Storage Account.

   ![Lab resource group is open. The storage account is highlighted.](media/select-storage-account.png "Storage Account Selection")

5. Switch to the **Access keys (1)** panel. Select **Show keys (2)** to reveal the keys. Select the copy button **(3)** for the first connection string and paste it to a text editor of your choice to be used in the following steps.

   ![Storage Account Access keys page is shown. The show Keys button is selected. The copy button for the first connection string is highlighted.](media/get-storage-connection-string.png "Copy Storage Connection String")

6. Go back to your resource group and select **contoso-portal-SUFFIX** App Service. This is the App Service hosting the hospital portal.

   ![Lab resource group is open. The App Service for the hospital portal is highlighted.](media/select-app-service.png "App Service Selection")

7. Switch to the **Configuration (1)** panel. Select **New application setting (2)**.

   ![App Service Configuration page is open. New application setting link is highlighted.](media/app-service-new-application-setting-step.png "App Service New Application Setting")

8. Set **Name (1)** to **ContosoStorageConnectionString** and **Value (2)** to the previously copied Azure Storage Connection String. Select **OK (3)** to save.

   ![Add Edit Application setting panel is open. Name is set to ContosoStorageConnectionString. Value is set to the previously copied Contoso storage account connection string. OK button is highlighted.](media/app-setting-config-storage.png "App Service Storage Application Setting")

9. Repeat the same steps to add the **Application Settings** listed below.

   | Name           | Value                                                                 |
   |----------------|-----------------------------------------------------------------------|
   | AzureSearchKey | Previously copied Endpoint **URL** for Cognitive Search               |
   | AzureSearchUrl | Previously copied Endpoint **Primary Admin Key** for Cognitive Search |

10. Once all settings **(1)** are set, select **Save (2)** and **Continue**.

    ![New application settings are highlighted. Save button is pointed.](media/app-service-settings-save-step.png "Save new application settings")

11. Go back to the **Overview (1)** page and select the **URL (2)** to navigate to the hospital portal.

    ![Overview page for the App Service is shown. URL for the App Service is highlighted.](media/app-service-navigate-to-portal.png "Hospital Portal URL")

12. Search for **covid** on the portal and observe results that include both claims documents and visit transcriptions. Feel free to use the filters based on medical information extracted by Cognitive Text Analytics for Health.

    ![Hospital Portal is shown. The search box is filled with COVID. Claims document and transcription results are highlighted. Filtering options based on medical information are highlighted.](media/hospital-portal.png "Hospital Portal")

## Exercise 4: Building custom PowerBI reports on healthcare data

Duration: X minutes

In this exercise, you will create Power BI reports surfacing the data extracted from claims forms and the health analytics data extracted from visit audio transcriptions. With the use of [DirectQuery](https://docs.microsoft.com/en-us/power-bi/connect-data/service-dataset-modes-understand#directquery-mode), our reports will send native queries to retrieve data from the underlying data source, in our case Cosmos DB. The ability to natively query Cosmos DB from custom reports is beneficial when reports need to deliver "near real-time" data beyond what can be achieved within scheduled refreshes.

### Task 1: Connecting PowerBI to CosmosDB

1. Connect to your LabVM and start **Power BI Desktop (1)** from its shortcut on the desktop. Select **Get data (2)** to continue.

   ![Power BI Desktop shortcut on desktop is highlighted. PowerBI window is shown. Get data link is highlighted.](media/powerbi-start.png "Power BI Desktop")

2. Type **cosmos (1)** into the search box and select **Azure Cosmos DB (2)** as the data source. Select **Connect (3)** to continue.

   ![Get data window is open. The search result for cosmos is shown. Azure Cosmos DB is selected. Connect button is highlighted.](media/powerbi-cosmosdb-connector.png "Get Data Cosmos DB")

3. Enter the **Cosmos DB URL** you previously copied into the **URL (1)** field. Type in **Contoso** for the **Database (2)** name. Select **OK (3)** to continue.

   ![Azure Cosmos DB Connection window is shown. URL is set to the Cosmos DB URL. The database is set to Contoso. The OK button is highlighted.](media/powerbi-cosmosdb-url.png "Cosmos DB Connection")

4. Enter the **Cosmos DB Primary Key** you previously copied into the **Account key (1)** field. Select **Connect (2)** to continue.

   ![Cosmos DB Feed Key window is open. The account key is set to Cosmos DB Primary Key. Connect button is highlighted.](media/powerbi-cosmosdb-key.png "Cosmos DB Account Key")

5. On the Navigator window select **Claims** and **Transcriptions** collections **(1)**. Select **Load (2)** to continue.

   ![Navigator window is shown. Claims and Transcriptions collections are selected. Load button is highlighted.](media/powerbi-load-cosmosdb.png "Loading Claims and Transcriptions")

6. So far, we did not decide what attributes in our JSON documents in Cosmos DB will be reflected as columns in Power BI. Select **Continue** and disregard the error to define the mapping on the following step.

   ![Load error is shown. Continue button is highlighted.](media/powerbi-cant-load.png "Loading Failed")

### Task 2: Setting up data transformations for semi-structured data

1. Select **Transform Data (1)** to start transforming our unstructured data.

   ![Power BI Desktop is open. Transform data menu is shown.](media/powerbi-transform-data.png "Transform Data")

2. Select **Claims** Query from the left Queries list **(1)**. Select the context  menu button **(2)** and open the field list. From the list select **TotalCharges, AmountPaid, AmountDue, DocumentDate** and **Diagnosis (3)**. Select **OK (4)** to finish.

   ![Claims query is selected. The context menu is open for the documents collection. From the fields list, TotalCharges, AmountPaid, AmountDue, DocumentDate, and Diagnosis are chosen. The OK button is highlighted. ](media/powerbi-claims-transform.png "Drill Down to Fields")

3. Once the data is loaded, it is time to assign the proper data types to columns. Right-click each column and assign the matching data type. For numbered columns such as **TotalCharges, AmountPaid** and **AmountDue** select **Decimal Number (3)**. For **DocumentDate** set **Date/Time**.

   ![TotalCharges column context menu is open. Change Type / Decimal Number selected is selected.](media/powerbi-change-data-types.png "Changing Data Types")

4. Now that the query is ready to be loaded, right-click **Claims (1)** from the query list and **Enable Load (2)**.

   ![Claims query is selected. Right-click context menu is shown. Enable Load is checked.](media/powerbi-claims-enable-load.png "Enable Load")

5. Now, we will switch gears and focus on the **Transcriptions** query in the Queries list. Right-click on the document list and open the context menu to select the **HealthcareEntities** field. Select **OK (3)** to continue.

   ![Transcriptions query is selected. The context menu is open for the documents collection. From the fields list, HealthcareEntities is selected. The OK button is highlighted. ](media/powerbi-transcriptions-healthcare-entities.png "Healthcare Entities from Transcriptions")

6. Select the arrow button **(1)** on the HealthcareEntities collection header and choose **Expand to New Rows (2)**.

   ![HealthcareEntities column context menu is open. Expand to new rows command is highlighted.](media/powerbi-expand-new-rows.png "Expand New Rows")

7. Select the arrow button **(1)** on the HealthcareEntities collection header and choose **Category** and **Text (2)** fields. Select **OK (3)** to continue.

   ![HealthcareEntities column context menu is open. Category and Text fields are selected. OK button is highlighted.](media/powerbi-transcriptions-select-fields.png "Selectin Fields for HealthcareEntities")

8. Right-click **Transcriptions (1)** from the query list and **Enable Load (2)**.

   ![Transcriptions query is selected. Right-click context menu is shown. Enable Load is checked.](media/powerbi-transcriptions-enable-load.png "Enable Load")

9. Select the arrow button **(1)** on the **HealthcareEntities.Category** field header and choose **Diagnosis (2)** to filter out other entities. We will only use entities under the **Diagnosis** category. Select **OK (3)** to continue.

   ![HealthcareEntities.Category column context menu is open. Diagnosis is selected. OK button is highlighted.](media/powerbi-transcriptions-filter.png "Filter Diagnosis")

10. Now, we will group by diagnosis texts to determine the number of times each diagnosis has appeared in audio recordings. Right-click **(1)** on the **HealthcareEntities.Text** field header and choose **Group By (2)**.

    ![HealthcareEntities.Text column context menu is open. Group By is highlighted.](media/powerbi-transcriptions-groupby.png "Group By")

11. Select **OK**.

    ![Group By window is shown. Ok button is highlighted.](media/powerbi-transcriptions-groupby-ok.png "Group By Details")

12. Close the window **(1)** and select **Yes (2)** to save changes.

    ![Close Window button is selected. Yes confirmation button is highlighted.](media/powerbi-save-transform.png "Save Transform")

### Task 3: Creating a custom PowerBI report from multiple data sets

1. Select **Treemap (1)** visualization and check **Transcriptions.Count** and **Document.HealthcareEntities.Text (2)** as values and grouping.

   ![Treemap visualization is selected. The count is set for Values and HealthcareEntities.Text is set for Group.](media/powerbi-treemap.png "Treemap")

2. Select **Stacked Bar Chart (1)** and check **Claims.Diagnosis (2)** as Axis, **Claims.TotalCharges (3)** as Values.

   ![Stacked Bar Chart visualization is selected. Claims.Diagnosis is set for Axis and Claims.TotalCharges is set for Values.](media/powerbi-bar-chart.png "Stacked Bar Chart")

3. Feel free to resize the charts. Here is an example view fetching data from visit audio recordings and claim submission forms.

   ![Report showing data from visit audio recordings and claim submission forms.](media/powerbi-sample-report.png "Sample Report")

## After the hands-on lab

Duration: 15 minutes

In this exercise, you will de-provision all Azure resources created in support of this hands-on lab.

### Task 1: Delete Azure resource groups

1. In the Azure portal, select **Resource groups** from the Azure services list and locate and delete the **hands-on-lab-SUFFIX** resource group.

You should follow all steps provided *after* attending the Hands-on lab.
