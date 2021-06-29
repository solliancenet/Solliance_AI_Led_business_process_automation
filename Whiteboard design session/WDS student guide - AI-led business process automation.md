![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/main/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
AI-led business process automation
</div>

<div class="MCWHeader2">
Whiteboard design session student guide
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

- [AI-led business process automation whiteboard design session student guide](#ai-led-business-process-automation-whiteboard-design-session-student-guide)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Step 1: Review the customer case study](#step-1-review-the-customer-case-study)
    - [Customer situation](#customer-situation)
    - [Customer needs](#customer-needs)
    - [Customer objections](#customer-objections)
    - [Infographic for common scenarios](#infographic-for-common-scenarios)
  - [Step 2: Design a proof of concept solution](#step-2-design-a-proof-of-concept-solution)
  - [Step 3: Present the solution](#step-3-present-the-solution)
  - [Wrap-up](#wrap-up)
  - [Additional references](#additional-references)

# AI-led business process automation whiteboard design session student guide

## Abstract and learning objectives

In this whiteboard design session, you will work in a group to automate the business process of extracting data from form documents and perform visit audio transcription (and translation where required) to extract and label medical information. You will evaluate Azure tools and services to design an optimal architecture to fulfill Contoso Healthcare's business process automation requirements.

At the end of this whiteboard design session, you will be better able to architect a solution to automate and enrich an existing business process and provide further insight into data using Azure Cognitive Services.

## Step 1: Review the customer case study

**Outcome**

Analyze your customer's needs.

Timeframe: 15 minutes

Directions: With all participants in the session, the facilitator/SME presents an overview of the customer case study along with technical tips.

1. Meet your table participants and trainer.

2. Read all of the directions for steps 1-3 in the student guide.

3. As a table team, review the following customer case study.

### Customer situation

Contoso Healthcare is a major hospital network consisting of multiple locations across the United States. One of Contoso Healthcare's most significant needs is to have the ability to process handwritten and electronically filled medical claims forms. Each hospital needs to provide filled forms to Contoso Healthcare's central offices in a standard fashion. Currently, claims forms are completed as both digital files and physical paper documents. Employees then review each document and enter data manually into the claims system. Contoso Healthcare is looking to automate the business process of obtaining claim forms, extracting claims form data to reduce overall form processing time, data-entry errors, and the loss of physical documents. Contoso can also then re-direct their employees to more impactful tasks and increase overall productivity.

In addition to medical claims form processing, Contoso is looking to automate the process of transcribing, translating, and storing patient/doctor visit audio recordings. Currently, each hospital records audio files of patient/physician visits. This data is archived on-premises at each hospital and used strictly as an auditing tool should the details of any visit be questioned. When the results of a patient visit are challenged, the recording of the visit is retrieved and audibly reviewed by hospital employees. Unfortunately, this manual review process is not standard across the hospital network. As a result, each hospital has its own methods of dealing with patient audio file storage, retrieval, and review. A translation may also be needed in addition to patient audio transcription when the visit language is Spanish. Currently, multiple language interpreters need to be on-hand at each hospital for the manual audio review process.

Contoso Healthcare wants to implement useful reporting visualizations over the extracted claims processing data, such as visualizing the ratio of total cost and the amount covered for a patient. Doctors are also interested in extracting critical insights from the patient visit audio transcriptions, preferably via search functionality available on their internal portal site.

### Customer needs

1. Claims forms and patient visit audio files need to be obtained from each hospital in the network consistently.

2. An automated process should extract data from claims forms submitted.

3. A report needs to be created to provide a visualization on total charges versus the amount paid for a specified date range obtained from claims form processing.

4. Audio of the patient visit must be transcribed.

5. If the patient visit audio is in Spanish, transcribed text must be translated into English (en-US).

6. Transcribed patient audio and claims forms must be made searchable from the internal web portal.

7. The process of extracting and storing data from claims forms and obtaining audio transcription from patient visits needs to be automated.

### Customer objections

1. Claims forms are filled out either electronically or are handwritten. We have a concern that handwritten input will not be able to be processed.

2. Patient visit audio may involve conversations in languages other than English. We need a solution that can identify and translate from Spanish into English (en-US). Additional languages might be needed as the network spans to other regions.

3. We want to extract insight from the audio transcriptions of patient visits through our internal portal searches. However, we don't have a data dictionary of medical terms. Is there a solution to analyze our audio transcripts to surface medical terminologies, such as dosages, medications, and diagnoses?

### Infographic for common scenarios

Using the sample labeling tool with Azure Form Recognizer to create a custom model to extract information from a form.

![The user interface of the sample labeling tool displays with and image of a form. The form has multiple fields highlighted. Details of the highlighted fields are displayed in the right blade along with their value extracted from the form.](media/formsrecognizer-sample-labeling-tool.png "Sample labeling tool")

A sample speech-to-text processing flow.

![A flowchart demonstrating audio triggering a speech-to-text activity to yield a document that can be translated and enriched using AI. The results of the processing to be stored in a data storage solution.](media/speech-to-text-processing.png "Speech-to-text sample flow")

## Step 2: Design a proof of concept solution

**Outcome**

Design a solution and prepare to present the solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 60 minutes

**Business needs**

Directions:  With all participants at your table, answer the following questions and list the answers on a flip chart:

1. Who should you present this solution to? Who is your target customer audience? Who are the decision makers?

2. What customer business needs do you need to address with your solution?

**Design**

Directions: With all participants at your table, respond to the following questions on a flip chart:

*File ingestion*

1. Each hospital must submit claims forms in a consistent manner. How do you suggest having each hospital provide claims forms for automated centralized processing?

2. Each hospital must provide patient audio files in a consistent manner. How do you suggest each hospital provide audio files for automated centralized processing? Does this differ from the method you suggested for claims forms? If so, why?

3. Audio and claim form files need to be stored centrally. What type of structure do you recommend to organize these incoming files?

4. The business process of extracting and storing claims form data and audio transcriptions must be automated. What do you recommend to trigger and orchestrate this processing, so that manual intervention is not required?

5. Once a claim form or audio file has been processed, how do you ensure that they do not get processed multiple times?

*Form processing*

1. What Azure service do you recommend to extract data from the claims forms? Are there any tools that can be used to simplify this process?

2. How do you recommend storing the data extracted from the claims forms?

*Reporting*

1. What Azure service do you recommend for the creation of reports to visualize data extracted from both the claims forms and audio transcriptions?

*Audio Transcription and translation*

1. What Azure service do you recommend for transcribing patient visit audio files?

2. How would you identify the spoken language of the visit?

3. What Azure service would you use to translate audio transcriptions to English (en-US)?

4. How do you recommend storing the audio transcription?

5. In case of an audit, how would you be able to track down the original source audio file for a specific transcription?

*Search indexing, enrichment, and implementation*

1. What Azure service do you recommend to index the audio transcription data to make them searchable?

2. How do you recommend keeping the index up-to-date when transcripts are added over time?

3. What Azure service do you recommend to enrich the search index to extract medical insights?

4. What Azure service do you recommend to rank search results based on the search criteria or to identify questions that may be asked and provide direct answers?

5. What steps must be taken to implement the audio transcription search to the internal web portal?

*High-level architecture*

1. Based on your answers to the questions above, diagram a high-level architecture for the initial vision of handling file ingestion, form processing, reporting, audio transcription/translation, as well as search indexing, enhancement, and implementation.

**Prepare**

Directions: With all participants at your table:

1. Identify any customer needs that are not addressed with the proposed solution.

2. Identify the benefits of your solution.

3. Determine how you will respond to the customer's objections.

Prepare a 15-minute chalk-talk style presentation to the customer.

## Step 3: Present the solution

**Outcome**

Present a solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 30 minutes

**Presentation**

Directions:

1. Pair with another table.

2. One table is the Microsoft team and the other table is the customer.

3. The Microsoft team presents their proposed solution to the customer.

4. The customer makes one of the objections from the list of objections.

5. The Microsoft team responds to the objection.

6. The customer team gives feedback to the Microsoft team.

7. Tables switch roles and repeat Steps 2-6.

## Wrap-up

Timeframe: 15 minutes

Directions: Tables reconvene with the larger group to hear the facilitator/SME share the preferred solution for the case study.

## Additional references

| Description                               | Links                                                                                                        |
|-------------------------------------------|--------------------------------------------------------------------------------------------------------------|
| Azure Storage Account                     | <https://docs.microsoft.com/en-us/azure/storage/common/storage-account-overview>                             |
| Azure Files                               | <https://docs.microsoft.com/en-us/azure/storage/files/storage-files-introduction>                            |
| Azure Event Grid                          | <https://docs.microsoft.com/en-us/azure/event-grid/overview>                                                 |
| ADLS Gen 2 Best Practices                 | <https://docs.microsoft.com/en-us/azure/storage/blobs/data-lake-storage-best-practices>                      |
| Azure Data Store Models                   | <https://docs.microsoft.com/en-us/azure/architecture/guide/technology-choices/data-store-overview>           |
| Azure Form Recognizer                     | <https://docs.microsoft.com/en-us/azure/cognitive-services/form-recognizer/overview?tabs=v2-1>               |
| Speech Service                            | <https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/overview>                          |
| Azure Cognitive Search                    | <https://docs.microsoft.com/en-us/azure/search/search-what-is-azure-search>                                  |
| Text Analytics for Health                 | <https://docs.microsoft.com/en-us/azure/cognitive-services/Text-Analytics/how-tos/text-analytics-for-health> |
| Semantic search in Azure Cognitive Search | <https://docs.microsoft.com/en-us/azure/search/semantic-search-overview>                                     |
| Power BI                                  | <https://docs.microsoft.com/en-us/power-bi/fundamentals/power-bi-overview>                                   |
| Azure Functions                           | <https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview>                                  |
