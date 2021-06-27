$subs = Get-AzSubscription | Select-Object -ExpandProperty Name
if($subs.GetType().IsArray -and $subs.length -gt 1){
    $subOptions = [System.Collections.ArrayList]::new()
    for($subIdx=0; $subIdx -lt $subs.length; $subIdx++){
            $opt = New-Object System.Management.Automation.Host.ChoiceDescription "$($subs[$subIdx])", "Selects the $($subs[$subIdx]) subscription."   
            $subOptions.Add($opt)
    }
    $selectedSubIdx = $host.ui.PromptForChoice('Enter the desired Azure Subscription for this lab','Copy and paste the name of the subscription to make your choice.', $subOptions.ToArray(),0)
    $selectedSubName = $subs[$selectedSubIdx]
    Write-Information "Selecting the $selectedSubName subscription"
    Select-AzSubscription -SubscriptionName $selectedSubName
}
$subscriptionId = (Get-AzContext).Subscription.Id
$dest = (Get-Item .).parent.FullName

$resourceGroupName = Read-Host -Prompt "Enter the name of the resource group you have created for your MCW environment."
$storageAccountName = (Get-AzureRmStorageAccount -ResourceGroupName $resourceGroupName).StorageAccountName
$storageAccountKey = (Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName -AccountName $storageAccountName).Value[0]
$destinationContext = New-AzStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey
$containerSASURI = New-AzStorageContainerSASToken -Context $destinationContext -ExpiryTime(get-date).AddSeconds(3600) -FullUri -Name "claimstraining" -Permission rw
$localPath = $dest + "/claims-forms/*"

Write-Host "Uploading training data to Azure Storage"

azcopy copy $localPath $containerSASURI --exclude-pattern="*test*.*"

Write-Host "Deploying Document Processing Azure Function App "
$functionAppName = (Get-AzFunctionApp -ResourceGroupName $resourceGroupName).Name
$zipArchiveFullPath = $dest + "/source-azure-functions/DocumentProcessing.zip"
az functionapp deployment source config-zip -g "$($resourceGroupName)" -n "$($functionAppName)" --src "$($zipArchiveFullPath)" --subscription "$($subscriptionId)"   

Write-Host "Deploying Hospital Portal"
$webAppName = (Get-AzWebApp -ResourceGroupName $resourceGroupName | Where-Object Name -like 'contoso-portal*').Name
$zipArchiveFullPath = $dest + "/source-hospital-portal/contoso-web.zip"
az webapp deployment source config-zip -g "$($resourceGroupName)" -n "$($webAppName)" --src "$($zipArchiveFullPath)" --subscription "$($subscriptionId)"   

Write-Host "Environment setup complete." -ForegroundColor Green

