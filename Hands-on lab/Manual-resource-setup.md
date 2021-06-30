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

## Task 1: Create the LabVM

In this task, you provision a virtual machine (VM) in Azure. The VM is used as a development machine to upload files to the Azure Portal and design PowerBI reports.

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

11. Run the code below to download the lab files and extract the content into `C:\MCW` folder.

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

    > **INFO**: You can copy multiline code from outside the RDP window and paste it into the terminal in the RDP session. In case of difficulty copying the content, go to the RDP terminal and right-click the top left terminal logo **(1)**. This will open a context menu where you can find Edit > Paste **(3)** command. After you paste the code, hit **Enter** to execute the final line.

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

    ![Storage Account's Containers page is open. +Container button is selected. New Container dialog is shown. Name is set to Claims. Create button is highlighted.](media/create-claims-container.png "Claims Container")

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

    ![Upload blob dialog is open. 20210621-test-form.pdf is selected. Upload button is highlighted.](media/storage-upload-claims-form.png "File Upload")
