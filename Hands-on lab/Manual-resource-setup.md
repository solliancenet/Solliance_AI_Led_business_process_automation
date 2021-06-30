# Manual resource setup guide

This guide provides step-by-step instructions to manually provision and configure the resources created by the ARM template and scripts used in the before the hands-on lab guide.

> **IMPORTANT**: Many Azure resources require globally unique names. Throughout these steps, the word "SUFFIX" appears as part of resource names. You should replace this with your Microsoft alias, initials, or another value to ensure resources are uniquely named.

June 2021

**Contents**:

- [Manual resource setup guide](#manual-resource-setup-guide)
  - [Task 1: Create the LabVM](#task-1-create-the-labvm)
  - [Task 2: Setting up the Lab VM](#task-2-setting-up-the-lab-vm)
  - [Task 3: Create an Azure Storage Account](#task-3-create-an-azure-storage-account)
  - [Task 4: Setting up the Azure Storage Account](#task-4-setting-up-the-azure-storage-account)
  - [Task 5: Create a Translator Account](#task-5-create-a-translator-account)
  - [Task 6: Create a Text Analytics Account](#task-6-create-a-text-analytics-account)
  - [Task 6: Create a Speech Account](#task-6-create-a-speech-account)
  - [Task 7: Create a Cognitive Search Account](#task-7-create-a-cognitive-search-account)
  - [Task 8: Create a Forms Recognizer Account](#task-8-create-a-forms-recognizer-account)
  - [Task 9: Create a Cosmos DB Account](#task-9-create-a-cosmos-db-account)
  - [Task 10: Create a Function App](#task-10-create-a-function-app)
  - [Task 11: Create a Web App](#task-11-create-a-web-app)
  - [Task 12: Deploy Web and Functions Apps](#task-12-deploy-web-and-functions-apps)

## Task 1: Create the LabVM

In this task, you provision a virtual machine (VM) in Azure. The VM is used as a development machine to upload files to the Azure Portal, and design PowerBI reports.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Create (1)** from the menu. Select **+Marketplace(2)** to continue.

    ![Resource group is shown. Create menu is open. Marketplace option is highlighted.](media/add-resource-from-marketplace.png "Add Resource")

2. Enter "Visual Studio 2019 **(1)**" into the Search the Marketplace box and open the **Create** menu for the **Visual Studio 2019 Latest (2)** option. Select **Visual Studio 2019 Enterprise (latest release) on Windows Server 2019 (x64) (3)** from the list.

   !["Visual Studio 2019" is entered into the Search the Marketplace box. Create menu for Visual Studio 2019 Latest is open. Visual Studio 2019 Enterprise (latest release) on Windows Server 2019 (x64) is highlighted.](media/select-visual-studio-2019-enterprise-vm.png "Visual Studio 2019 Latest")

3. On the Create a virtual machine **Basics** tab, set the following configuration:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the **hands-on-lab-SUFFIX** resource group from the list of existing resource groups.

   - Instance Details:

     - **Virtual machine name (3)**: Enter LabVM.
     - **Region (4)**: Select the region you are using for resources in this hands-on lab.
     - **Availability options**: Select no infrastructure redundancy required.
     - **Image (5)**: Select **Visual Studio 2019 Enterprise (latest release) on Windows Server 2019 (x64) - Gen1**.
     - **Azure Spot instance**: Select No.
     - **Size (6)**: Select Standard_D2s_v3.

   - Administrator Account:

     - **Username (7)**: Enter **demo**.
     - **Password (8)**: Enter **Password.1!!**

   - Inbound Port Rules:

     - **Public inbound ports (9)**: Choose Allow selected ports.
     - **Select inbound ports**: Select RDP (3389) in the list.

   ![Screenshot of the Basics tab, with fields set to the previously mentioned settings.](media/lab-virtual-machine-basics-tab.png "Create a virtual machine Basics tab")

   > **Note**: Default settings are used for the remaining tabs so that they can be skipped.

4. Select **Review + create (10)** to validate the configuration.

5. On the **Review + create** tab, ensure the Validation passed message is displayed, and then select **Create** to provision the virtual machine.

## Task 2: Setting up the Lab VM

1. In the [Azure portal](https://portal.azure.com), navigate to your **LabVM** Virtual Machine by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **LabVM** Virtual Machine from the list of resources.

    ![The WebVM virtual machine is highlighted in the list of resources.](media/select-labvm.png "WebVM Selection")

2. On the LabVM Virtual Machine's **Overview** blade, select **Connect (1)** and **RDP (2)** on the top menu.

   ![The LabVM VM blade is displayed, with the Connect button highlighted in the top menu.](media/connect-rdp-labvm.png "LabVM RDP Connect")

3. Select **Download RDP File** on the next page, and open the downloaded file.

    ![RDP Window is open. Download RDP File button is highlighted.](media/rdp-download.png "LabVM RDP File Download")

4. Select **Connect** on the Remote Desktop Connection dialog.

    ![In the Remote Desktop Connection Dialog Box, the Connect button is highlighted.](media/remote-desktop-connection-labvm.png "Remote Desktop Connection dialog")

5. Enter the following credentials with your password when prompted, and then select **OK**:

   - **Username**: demo
   - **Password**: Password.1!!

    ![The credentials specified above are entered into the Enter your credentials dialog.](media/rdp-credentials-labvm.png "Enter your credentials")

6. Select **Yes** to connect if prompted that the remote computer's identity cannot be verified.

    ![In the Remote Desktop Connection dialog box, a warning states that the remote computer's identity cannot be verified and asks if you want to continue anyway. At the bottom, the Yes button is circled.](media/remote-desktop-connection-identity-verification-labvm.png "Remote Desktop Connection dialog")

7. Once logged in, launch the **Server Manager**. This should start automatically, but you can access it via the Start menu if it does not.

8. Select **Local Server (1)**, then select **On (2)** next to **IE Enhanced Security Configuration**. In the Internet Explorer Enhanced Security Configuration dialog, select **Off (3) (4)** under both Administrators and Users, and then select **OK (5)**.

    ![Screenshot of the Server Manager. In the left pane, Local Server is selected. In the right, Properties (For LabVM) pane, the IE Enhanced Security Configuration, which is set to On, is highlighted. Internet Explorer Enhanced Security Configuration dialog box is shown, with Administrators and Users set to Off.](./media/windows-server-manager-ie-enhanced-security-configuration.png "Server Manager")

9. Close the Server Manager.

10. Right-click the Windows Start Menu and select **Windows PowerShell (Admin)** to start a terminal session.

    ![Windows Start Menu is highlighted. The context menu is open, and Windows Powershell (Admin) is selected.](media/run-powershell-admin.png "Windows PowerShell")

11. Run the code below to download the lab files and extract the content into the `C:\MCW` folder.

    ```PS
    $branchName = "main"
    New-Item -ItemType directory -Path C:\MCW
    while((Get-ChildItem -Directory C:\MCW | Measure-Object).Count -eq 0 )
    {
        (New-Object System.Net.WebClient).DownloadFile("https://github.com/solliancenet/Solliance_AI_Led_business_process_automation/zipball/$branchName", 'C:\MCW.zip')
        Expand-Archive -LiteralPath 'C:\MCW.zip' -DestinationPath 'C:\MCW' -Force
    }
    $item = get-item "c:\mcw\*"
    Rename-Item $item -NewName "MCW-$branchName"
    ```

    > **INFO**: You can copy multiline code from outside the RDP window and paste it into the terminal in the RDP session. In case of difficulty copying the content, go to the RDP terminal and right-click the top left terminal logo **(1)**. This will open a context menu where you can find the Edit > Paste **(3)** command. After you paste the code, hit **Enter** to execute the final line.

    ![Terminal menu is open. Edit > Paste command is highlighted.](media/paste-terminal.png "Pasta to terminal")

12. Download and install the tools listed below into the LabVM.

    | Name             | Link                                                                                                                                            |
    |------------------|-------------------------------------------------------------------------------------------------------------------------------------------------|
    | Microsoft Edge   | <https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/e2d06b69-9e44-45e1-bdf5-b3b827fe06b2/MicrosoftEdgeEnterpriseX64.msi> |
    | Git              | <https://github.com/git-for-windows/git/releases/download/v2.32.0.windows.1/Git-2.32.0-64-bit.exe>                                              |
    | Power BI Desktop | <https://download.microsoft.com/download/8/8/0/880BCA75-79DD-466A-927D-1ABF1F5454B0/PBIDesktopSetup_x64.exe>                                    |

## Task 3: Create an Azure Storage Account

In this task, you provision an Azure Storage account used to store and serve Claims Forms and Audio recordings. Additionally, the storage account will hold Forms Recognizer training data and serve as a backing Storage Account for Azure Functions.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "storage account **(1)**" into the Search the Marketplace box, and select **Storage account** from the results, and then select **Create (2)**.

   !["Storage account" is entered into the Search the Marketplace box. A storage account is selected in the results.](media/create-resource-storage-account.png "Create Storage account")

3. On the Create storage account **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Storage account name (3)**: Enter contosoSUFFIX.
     - **Location (4)**: Select the location you are using for resources in this hands-on lab.
     - **Performance (5)**: Choose **Standard**.
     - **Replication (6)**: Select **Locally-redundant storage (LRS)**.

   ![On the Create storage account blade, the values specified above are entered into the appropriate fields.](media/storage-create-account.png "Create storage account")

4. Select **Review + create (7)**.

5. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**. Once deployment is complete, select **Go To Resource** to navigate to the Storage Account.

## Task 4: Setting up the Azure Storage Account

1. Switch to the **Resource sharing (CORS) (1)** page and add a new Blob service CORS rule adding the values below. Once you are done, select **Save (5)**.

    | Allowed origins | Allowed methods | Allowed headers |
    |-----------------|-----------------|-----------------|
    | *               | Select All      | *               |

    ![Storage account's Resource Sharing (CORS) page is open. A new rule for the Blob Service is defined. Allowed origins is set to *. All allowed methods are selected. Allowed headers is set to *. Save button is highlighted.](media/storage-cors-setting.png "Storage Blob Service CORS Settings")

2. Switch to the **Containers (1)** page and select **+Container (2)**. On the **New container** dialog set **Name** to **claims (3)**. Select **Create (4)** to create the new container.

    ![Storage Account's Containers page is open. +Container button is selected. New Container dialog is shown. The name is set to Claims. Create button is highlighted.](media/create-claims-container.png "Claims Container")

3. Repeat the previous container creation steps for containers **claimstraining** and **audiorecordings** containers.

4. In the [Azure portal](https://portal.azure.com), navigate to your **LabVM** Virtual Machine by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **LabVM** Virtual Machine from the list of resources.

    ![The WebVM virtual machine is highlighted in the list of resources.](media/select-labvm.png "WebVM Selection")

5. On the LabVM Virtual Machine's **Overview** blade, select **Connect (1)** and **RDP (2)** on the top menu.

    ![The LabVM VM blade is displayed, with the Connect button highlighted in the top menu.](media/connect-rdp-labvm.png "LabVM RDP Connect")

6. Select **Download RDP File** on the next page, and open the downloaded file.

    ![RDP Window is open. Download RDP File button is highlighted.](media/rdp-download.png "LabVM RDP File Download")

7. Select **Connect** on the Remote Desktop Connection dialog.

    ![In the Remote Desktop Connection Dialog Box, the Connect button is highlighted.](media/remote-desktop-connection-labvm.png "Remote Desktop Connection dialog")

8. Enter the following credentials with your password when prompted, and then select **OK**:

    - **Username**: demo
    - **Password**: Password.1!!

    ![The credentials specified above are entered into the Enter your credentials dialog.](media/rdp-credentials-labvm.png "Enter your credentials")

9. Select **Yes** to connect if prompted that the remote computer's identity cannot be verified.

    ![In the Remote Desktop Connection dialog box, a warning states that the remote computer's identity cannot be verified and asks if you want to continue anyway. At the bottom, the Yes button is circled.](media/remote-desktop-connection-identity-verification-labvm.png "Remote Desktop Connection dialog")

10. Once logged in, launch **Edge** and navigate to the [Azure portal](https://portal.azure.com). Enter your credentials to access your subscriptions. Navigate to **contosoSUFFIX** storage account by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **contosoSUFFIX** Storage Account from the list of resources.

    ![Edge is highlighted on the desktop. Browser is open and navigated to portal.azure.com. Storage account overview page is open.](media/azure-portal-labvm.png "Storage Account on Lab VM")

11. Switch to the **Containers (1)** panel. Select the **claims (2)** container.

    ![Contoso storage containers are listed. Claims container is highlighted.](media/storage-claimstraining-container.png "Claims Storage Container")

12. Select **Upload (1)** and **Browse (2)**. Navigate to `C:\MCW\MCW-main\Hands-on lab\lab-files\claims-forms` **(3)**. Pick **20210621-test-form (4)** and select **Open (5)**. This PDF file is brand new in the eyes of our trained model and not used during model training.

    ![Container page is open. The upload button is selected. File open dialog shows the claims-forms folder with PDF files listed. 20210621-test-form PDF file and Open button are highlighted.](media/upload-training-claims-form.png "Local file selection for upload.")

13. Select **Upload** to start the upload process.

## Task 5: Create a Translator Account

In this task, you provision a Translator account used to translate transcriptions to English.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Translator **(1)**" into the Search the Marketplace box, and select **Translator** from the results, and then select **Create (2)**.

   !["Translator" is entered into the Search the Marketplace box. A Translator account is selected in the results.](media/create-resource-translator-account.png "Create Translator account")

3. On the Create Translator account **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Region (3)**: Select the location you are using for resources in this hands-on lab.
     - **Name (4)**: Enter contoso-translate-SUFFIX.
     - **Pricing Tier (5)**: Choose **Standard S1**.

   ![On the Create Translator account blade, the values specified above are entered into the appropriate fields.](media/translator-create-account.png "Create translator account")

4. Select **Review + create (6)**.

5. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 6: Create a Text Analytics Account

In this task, you provision a Text Analytics account used to extract medical information from claims documents.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Text Analytics **(1)**" into the Search the Marketplace box, and select **Text Analytics** from the results, and then select **Create (2)**.

   !["Text Analytics" is entered into the Search the Marketplace box. A Text Analytics account is selected in the results.](media/create-resource-text-analytics-account.png "Create Text Analytics account")

3. Select **Continue to create your resource** when asked for additional features.

4. On the Create Text Analytics account **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Region (3)**: Select the location you are using for resources in this hands-on lab.
     - **Name (4)**: Enter contoso-textanalytics-SUFFIX.
     - **Pricing Tier (5)**: Choose **Standard S**.
     - Check the Responsible AI Notice **(6)**

   ![On the Create Text Analytics account blade, the values specified above are entered into the appropriate fields.](media/text-analytics-create-account.png "Create Text Analytics account")

5. Select **Review + create (7)**.

6. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 6: Create a Speech Account

In this task, you provision a Speech account used to detect the language of voice recordings and do transcriptions.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Speech **(1)**" into the Search the Marketplace box, and select **Speech** from the results, and then select **Create (2)**.

   !["Speech" is entered into the Search the Marketplace box. A Speech account is selected in the results.](media/create-resource-speech-account.png "Create Speech account")

3. On the create Speech account page, enter the following:

   - **Name (1)**: Enter contoso-speech-SUFFIX.
   - **Subscription (2)**: Select the subscription you are using for this hands-on lab.
   - **Location (3)**: Select the location you use for resources in this hands-on lab.
   - **Pricing Tier (4)**: Choose **Standard S0**.
   - **Resource Group (5)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   ![On the Create Speech account blade, the values specified above are entered into the appropriate fields.](media/speech-create-account.png "Create Speech account")

4. Select **Review + create (6)**.

5. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 7: Create a Cognitive Search Account

In this task, you provision a Cognitive Search account used to search for claims and audio transcriptions data through the hospital portal.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Search **(1)**" into the Search the Marketplace box and select **Azure Cognitive Search** from the results, and then select **Create (2)**.

   !["Search" is entered into the Search the Marketplace box. A Cognitive Search account is selected in the results.](media/create-resource-cognitive-search-account.png "Create Cognitive Search account")

3. On the New Search Service page **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Service Name (3)**: Enter contoso-search-SUFFIX.
     - **Region (4)**: Select the location you are using for resources in this hands-on lab.
     - **Pricing Tier (5)**: Choose **Standard**.

   ![On the New Search Service account blade, the values specified above are entered into the appropriate fields.](media/search-create-account.png "Create new Search Service account")

4. Select **Review + create (6)**.

5. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 8: Create a Forms Recognizer Account

In this task, you provision a Forms Recognizer account used to search extract structured information from claims forms.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Forms Recognizer **(1)**" into the Search the Marketplace box, and select **Forms Recognizer** from the results, and then select **Create (2)**.

   !["Forms Recognizer" is entered into the Search the Marketplace box. A Forms Recognizer account is selected in the results.](media/create-resource-forms-recognizer-account.png "Create Forms Recognizer account")

3. On the Create Forms Recognizer page **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Region (3)**: Select the location you are using for resources in this hands-on lab.
     - **Name (4)**: Enter contoso-fr-SUFFIX.
     - **Pricing Tier (5)**: Choose **Standard S0**.

   ![On the New Forms Recognizer Service account blade, the values specified above are entered into the appropriate fields.](media/forms-recognizer-create-account.png "Create new Forms Recognizer Service account")

4. Select **Review + create (6)**.

5. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 9: Create a Cosmos DB Account

In this task, you provision a Cosmos DB account to store claims and audio transcription data sets accessed by custom Power BI reports.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Cosmos DB **(1)**" into the Search the Marketplace box, and select **Azure Cosmos DB** from the results, and then select **Create (2)**.

   !["Cosmos DB" is entered into the Search the Marketplace box. An Azure Cosmos DB account is selected in the results.](media/create-resource-cosmos-db-account.png "Create Azure Cosmos DB account")

3. On the Select API Option page select **Create** for **Core (SQL) - Recommended** option.

   !["Cosmos DB API Option selection page is open. Create button for Core SQL is highlighted.](media/select-cosmosdb-api.png "Select Core SQL for Cosmos DB")

4. On the Create Forms Recognizer page **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Account Name (3)**: Enter contoso-cdb-SUFFIX.
     - **Location (4)**: Select the location you are using for resources in this hands-on lab.
     - **Capacity Mode (5)**: Choose **Serverless**.

   ![On the Create Azure Cosmos DB account blade, the values specified above are entered into the appropriate fields.](media/cosmos-db-create-account.png "Create new Azure Cosmos DB account")

5. Select **Review + create (6)**.

6. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 10: Create a Function App

In this task, you provision a Function App service used to host the functions processing claims and audio files using various Cognitive Services.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Function App **(1)**" into the Search the Marketplace box, and select **Function App** from the results, and then select **Create (2)**.

   !["Function App" is entered into the Search the Marketplace box. A Function App is selected in the results.](media/create-resource-function-app.png "Create Function App")

3. On the Create Function App page **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Function App name(3)**: Enter contoso-func-SUFFIX.
     - **Publish (4)**: Select **Code**.
     - **Runtime stack (5)**: Select **.NET**.
     - **Version (6)**: Select **3.1**.
     - **Region (7)**: Select the location you are using for resources in this hands-on lab.

   ![On the Create Function App blade, the values specified above are entered into the appropriate fields.](media/function-app-create-account.png "Create new Function App")

4. Select **Review + create (8)**.

5. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 11: Create a Web App

In this task, you provision a Web App service used to host the hospital portal.

1. In the [Azure portal](https://portal.azure.com/), go to your hands-on-lab-SUFFIX resource group and select **+Add (1)** from the menu.

   ![hands-on-lab-SUFFIX resource group is displayed. +Add button is highlighted in the portal menu.](media/add-resource-from-marketplace.png "Create a resource")

2. Enter "Web App **(1)**" into the Search the Marketplace box, and select **Web App** from the results, and then select **Create (2)**.

   !["Web App" is entered into the Search the Marketplace box. A Web App is selected in the results.](media/create-resource-web-app.png "Create Web App")

3. On the Create Web App page **Basics** tab, enter the following:

   - Project Details:

     - **Subscription (1)**: Select the subscription you are using for this hands-on lab.
     - **Resource Group (2)**: Select the hands-on-lab-SUFFIX resource group from the list of existing resource groups.

   - Instance Details:

     - **Name (3)**: Enter contoso-portal-SUFFIX.
     - **Publish (4)**: Select **Code**.
     - **Runtime stack (5)**: Select **.NET 5**.
     - **Operating System (6)**: Select **Windows**.
     - **Region (7)**: Select the location you are using for resources in this hands-on lab.

   - App Service Plan:

     - **Sku and size (8)**: Select **Basic B1**.

   ![On the Create Web App blade, the values specified above are entered into the appropriate fields.](media/web-app-create-account.png "Create new Web App")

4. Select **Review + create (9)**.

5. On the **Review + create** blade, ensure the Validation passed message is displayed and then select **Create**.

## Task 12: Deploy Web and Functions Apps

In this task, you will deploy the web and functions app into the web app and function app services previously provisioned.

1. In the Azure Portal, open the Azure Cloud Shell by selecting its icon from the right side of the top toolbar. Be sure to select **Powershell** as the shell type.

    ![A portion of the Azure Portal taskbar is displayed with the Cloud Shell icon highlighted.](media/bhol_azurecloudshellmenu.png "Opening the Cloud Shell")

    > **Note**: If you are prompted to create a storage account for the Cloud Shell, agree to have it created.

2. In the Cloud Shell window, enter the following command to download and deploy the function app.

    ```PowerShell
    (New-Object System.Net.WebClient).DownloadFile('https://github.com/solliancenet/Solliance_AI_Led_business_process_automation/raw/main/Hands-on%20lab/lab-files/source-azure-functions/DocumentProcessing.zip', 'Document.Zip')
    az functionapp deployment source config-zip -g "hands-on-lab-SUFFIX" -n "contoso-func-SUFFIX" --src "Document.Zip" --subscription "YOUR-SUBSCRIPTION-ID-COMES-HERE"   
    ```

3. In the Cloud Shell window, enter the following command to download and deploy the web app.

    ```PowerShell
    (New-Object System.Net.WebClient).DownloadFile('https://github.com/solliancenet/Solliance_AI_Led_business_process_automation/raw/main/Hands-on%20lab/lab-files/source-hospital-portal/contoso-web.zip', 'contoso-web.zip')
    az webapp deployment source config-zip -g "hands-on-lab-SUFFIX" -n "contoso-portal-SUFFIX" --src "contoso-web.zip" --subscription "YOUR-SUBSCRIPTION-ID"  
    ```
