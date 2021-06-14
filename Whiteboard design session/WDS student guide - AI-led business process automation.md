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

In this whiteboard design session, you will work in a group to automate the business process of extracting data from form documents and perform visit audio transcription (and translation where required). You will evaluate Azure tools and services to design an optimal architecture that will fulfill Contoso Healthcare's business process automation requirements.

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

Contoso Healthcare is a major hospital network consisting of multiple locations across the United States. One of Contoso Healthcare's most significant needs is to have the ability to process handwritten and electronically filled medical claims forms. Currently, each hospital uploads image representations of completed forms via an Azure File Share. Employees then review each document and enter data manually into the claims system.

Contoso Healthcare is looking to automate the business process of extracting claims form data to reduce overall form processing time and data-entry errors. Contoso can also then re-direct their employees to more impactful tasks and increase overall productivity.

In addition to medical claims form processing, Contoso is looking to automate the process of transcribing, translating, and storing patient/doctor visit audio recordings. Currently, each hospital uploads audio files via an Azure File Share, and this data is used strictly as an auditing tool should any clarity requests be made. If the clarity of a visit is requested, these recordings are retrieved and audibly reviewed by employees to interpret and obtain the details requested. Therefore, a key benefit to getting audio transcripts from these visits is the ability to surface valuable medical terms, dosage instructions, and diagnoses discussed. In addition to transcription and data extraction, the language of the patient visit audio should also be translated if required and stored in the system as the default of US English (en-us).

From an end-user perspective, Contoso Healthcare wants to implement useful reporting visualizations over the extracted claims processing data. In addition, concerning the patient visit audio transcription and analysis, they wish to augment the search capabilities of their existing internal web portal to surface essential information such as medical terms, dosage instructions, and diagnoses discussed in patient visits.

### Customer needs

1. Claims forms are being provided as image files. Data needs to be extracted from form fields and stored.

2. A report needs to be created to provide a visualization on total charges versus the amount paid for a specified date range.

3. Patient visit audio recordings must be transcribed.

4. If the patient visit audio is not in English. Transcribed text must be translated into English (en-US).

5. Transcribed patient audio must be indexed so that it can be searched from the internal web portal.

### Customer objections

1. Claims forms are filled out either electronically or are handwritten. We have a concern that handwritten input will not be able to be processed.

2. Patient visit audio may involve conversations in languages other than English. We need a solution that can identify and transcribe from multiple languages.

3. We don't have a data dictionary of medical terms, dosages, and diagnoses. We don't want to spend months analyzing patient visit transcripts to build data sufficient enough to train a machine learning model that will be used to extract this key information from future transcripts. Is there an existing solution for this?

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

1.  Who should you present this solution to? Who is your target customer audience? Who are the decision makers?

2.  What customer business needs do you need to address with your solution?

**Design**

Directions: With all participants at your table, respond to the following questions on a flip chart:

*Title*

1.  Number and insert questions here

*Title*

1.  Number and insert questions here

**Prepare**

Directions: With all participants at your table:

1.  Identify any customer needs that are not addressed with the proposed solution.

2.  Identify the benefits of your solution.

3.  Determine how you will respond to the customer's objections.

Prepare a 15-minute chalk-talk style presentation to the customer.

## Step 3: Present the solution

**Outcome**

Present a solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 30 minutes

**Presentation**

Directions:

1.  Pair with another table.

2.  One table is the Microsoft team and the other table is the customer.

3.  The Microsoft team presents their proposed solution to the customer.

4.  The customer makes one of the objections from the list of objections.

5.  The Microsoft team responds to the objection.

6.  The customer team gives feedback to the Microsoft team.

7.  Tables switch roles and repeat Steps 2-6.

##  Wrap-up 

Timeframe: 15 minutes

Directions: Tables reconvene with the larger group to hear the facilitator/SME share the preferred solution for the case study.

##  Additional references

|    |            |
|----------|:-------------:|
| **Description** | **Links** |
|   |   |
|   |   |
|   |   |
|   |   |
