![Microsoft Cloud Workshops](https://raw.githubusercontent.com/Microsoft/MCW-Template-Cloud-Workshop/main/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
[Insert workshop name here]
</div>

<div class="MCWHeader2">
Hands-on lab step-by-step
</div>

<div class="MCWHeader3">
[Insert date here Month Year]
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2021 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [\[insert workshop name here\] hands-on lab step-by-step](#\insert-workshop-name-here\-hands-on-lab-step-by-step)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Overview](#overview)
  - [Solution architecture](#solution-architecture)
  - [Requirements](#requirements)
  - [Exercise 1: Extract Text and Structure from Documents with Forms Recognizer](#exercise-1-exercise-name)
    - [Task 1: Prepare Custom Model to process documents](#task-1-task-name)
    - [Task 2: Configure Azure Functions for document processing](#task-2-task-name)
    - [Task 3: Setting up a Synapse Pipeline for data flow management](#task-2-task-name)
  - [Exercise 2: Implementing Semantic Search for the web site](#exercise-2-exercise-name)
    - [Task 1: Moving data to Azure Cognitive Search with Synapse](#task-1-task-name-1)
    - [Task 2: Enabling Semantic Search for the Search Index](#task-2-task-name-1)
    - [Task 3: Configure the web portal to use semantic ranking and AI summarization](#task-2-task-name)
  - [Exercise 3: Data Enrichment with PII Masking Cognitive Skill](#exercise-2-exercise-name)
    - [Task 1: Configure PII Masking Skill and Indexer](#task-1-task-name-1)
    - [Task 2: Running the Indexer](#task-2-task-name-1)
    - [Task 3: Configure the web portal to use enrichment data](#task-2-task-name)
  - [Exercise 4: Setting up portal web site](#exercise-2-exercise-name)
    - [Task 1: Moving data to CosmosDB with Synapse](#task-1-task-name-1)
    - [Task 2: Connecting the web portal to CosmosDB](#task-2-task-name-1)
  - [Exercise 5: Building Customer Sentiment Reporting](#exercise-2-exercise-name)
    - [Task 1: Moving data to a Synapse Spark Table](#task-1-task-name-1)
    - [Task 2: Implementing Sentiment Analysis in Synapse](#task-2-task-name-1)
    - [Task 3: Creating Sentiment Report](#task-2-task-name)
  - [After the hands-on lab](#after-the-hands-on-lab)
    - [Task 1: Delete resource group](#task-1-task-name-3)

<!-- /TOC -->

# \[insert workshop name here\] hands-on lab step-by-step

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

A second function in the Function App processes audio recordings. Contoso uses [Azure Cognitive Speech Audio Language Identification](cognitive-services/speech-service/how-to-automatic-language-detection?pivots=programming-language-csharp) to detect the language of the audio file and transcribe it to text. Once the text transcriptions are ready, Spanish transcriptions are translated to English using [Azure Cognitive Services Text Translator](https://azure.microsoft.com/en-us/services/cognitive-services/translator/). Finally, [Azure Cognitive Services Text Analytics for Health](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/how-tos/text-analytics-for-health?tabs=ner) is used to extract and label relevant medical information to provide a richer search experience in the internal hospital portal. Once the results are ready, the function saves the data in an Azure CosmosDB collection to be indexed by Azure Cognitive Search.

Finally, the internal hospital portal queries the indexes created in Azure Cognitive Search, offering a unified search experience for both structured and unstructured data sets.

## Requirements

- Microsoft Azure subscription must be pay-as-you-go or MSDN.
  - Trial subscriptions will _not_ work.

## Before the hands-on lab

Refer to the Before the hands-on lab setup guide manual before continuing to the lab exercises.

## Exercise 1: Exercise name

Duration: X minutes

\[insert your custom Hands-on lab content here . . . \]

### Task 1: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -  

### Task 2: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -  

## Exercise 2: Exercise name

Duration: X minutes

\[insert your custom Hands-on lab content here . . . \]

### Task 1: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -  

### Task 2: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -  

## Exercise 3: Exercise name

Duration: X minutes

\[insert your custom Hands-on lab content here . . .\]

### Task 1: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -  

### Task 2: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -  

## After the hands-on lab

Duration: X minutes

\[insert your custom Hands-on lab content here . . .\]

### Task 1: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -  

### Task 2: Task name

1. Number and insert your custom workshop content here . . .

    - Insert content here

        -

You should follow all steps provided *after* attending the Hands-on lab.
