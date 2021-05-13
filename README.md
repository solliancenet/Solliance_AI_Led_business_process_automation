# AI-led business process automation

Trey Research Inc. is a prominent specialized healthcare group consisting of multiple participating physician offices throughout the United States.  During the intake process, a physician interviews patients to collect a brief health history and preliminary observations regarding their general mood toward healthcare services rendered in the past. This data helps identify potential candidates for various medicinal studies. Typically a patient with a more positive view of healthcare is more apt to participate in these critical studies. Furthermore, the health history provides insight into which studies may apply to the patient.

Intake form documents are provided to Trey Research Inc. in PDF format and uploaded to an Azure storage account via an existing SFTP process. They currently employ dozens of interns to input this data manually into their system. Trey Research is looking to automate the business process of ingesting and processing this form data while adding flexible search functionality to their existing hospital portal. They have concerns regarding divulging PII in search results to less privileged users. They also need a high-level dashboard to keep them informed of the ratio of patients presenting with a positive versus negative outlook to their healthcare services.

June 2021

## Target audience

- Application developer
- AI developer
- Data engineer
- Data architect

## Abstracts

### Workshop

In this workshop, you will learn to automate a business process end-to-end using a variety of Azure Cognitive Services. First, you will train a Form Recognizer model to extract data from intake form documents. You will build an Azure Synapse Analytics pipeline to exercise this model that provides data to downstream systems. You will create a Cognitive Search index and enrich this data with a skill to mask PII. Applying Semantic Search will augment this index to provide more insightful search results. You will integrate this search functionality into the hospital portal web application. In addition to search, you will learn how to leverage the integration of Cognitive Services with Azure Synapse Analytics by enriching processed data with sentiment analysis to be visualized through a Power BI report.

At the end of this workshop, you will be better able to architect and implement a business process automation solution that leverages Azure Cognitive Services.

### Whiteboard design session

In this whiteboard design session, you will work in a group to automate the business process of extracting data from intake form documents. You will leverage AI to process and enrich this data to meet the goals of Trey Research Inc. You will also evaluate Azure tools and services to design an optimal architecture that will fulfill their needs. Because PII may be present in the intake form, you will need to ensure raw data is secure from unauthorized access. You will guide Trey Research Inc. to include powerful search capabilities into their design, paying particular attention to not divulging PII data. You will also need to include provisions to support a high-level sentiment analysis report.

At the end of this whiteboard design session, you will be better able to architect a solution to automate and enrich an existing business process and provide further insight into data using Azure Cognitive Services.

### Hands-on lab

In hands-on lab, you will learn to train a Form Recognizer model to extract data from intake form documents. You will build an Azure Synapse Analytics pipeline to automate this business process that provides pertinent data to downstream systems. The data extracted is enriched with a skill to mask PII and made available through a cognitive search index. Semantic Search capabilities further augment this index with semantic relevance and language understanding to provide more insightful search results. You will integrate this search functionality into the hospital portal web application. During the document processing pipeline execution, the integration of Azure Synapse Analytics and Cognitive Services further demonstrates data enrichment by adding sentiment analysis. A serverless SQL pool table exposes the sentiment data to a Power BI report to visualize the ratio of positive-minded patients to neutral or negative-minded individuals.

At the end of this hands-on lab, you will be better able to implement a business process automation solution that leverages Azure Cognitive Services.

## Azure services and related products

- App Service
- Cognitive Services: Form Recognizer
- Cognitive Services: Text Analytics - Sentiment Analysis
- Cognitive Search and Semantic Search
- Function App
- Azure Data Lake Storage Gen2
- Azure Synapse Analytics
- Apache Spark in Azure Synapse Analytics
- Cosmos DB
- Power BI
- Visual Studio Code

## Related references

![The solution architecture diagram as described in the paragraph that follows.](Media/architecture.png "Solution architecture")

Raw data is provided via SFTP process and is dropped in an ADLS Gen 2 storage account. An Azure Synapse Analytics pipeline initiates an Azure Functions activity that leverages a trained Form Recognizer model to extract data from the forms. The result of the data extraction is stored in an ADLS Gen 2 container. Another pipeline activity initiates a Synapse Notebook that leverages the Cognitive Services integration to enrich extracted data with sentiment analysis and stores the result in a Spark table. This table is exposed to a Power BI sentiment report via a serverless SQL Pool. A Cognitive Search indexer indexes the extracted data in storage (from the first activity) and enriches it by applying the PII skill and stores the result in an ADLS Gen 2-based knowledgestore. This search is then integrated into the hospital portal web application.

## Help & Support

We welcome feedback and comments from Microsoft SMEs & learning partners who deliver MCWs.  

***Having trouble?***
- First, verify you have followed all written lab instructions (including the Before the Hands-on lab document).
- Next, submit an issue with a detailed description of the problem.
- Do not submit pull requests. Our content authors will make all changes and submit pull requests for approval.  

If you are planning to present a workshop, *review and test the materials early*! We recommend at least two weeks prior.

### Please allow 5 - 10 business days for review and resolution of issues.
