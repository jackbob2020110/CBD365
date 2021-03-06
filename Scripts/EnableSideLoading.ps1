 
$programFiles = [environment]::getfolderpath("programfiles")
add-type -Path $programFiles'\SharePoint Online Management Shell\Microsoft.Online.SharePoint.PowerShell\Microsoft.SharePoint.Client.dll'
Write-Host 'Enabling SharePoint app sideLoading'
 
$siteurl = "[YOUR_DOMAIN].sharepoint.com"
$username = "[YOUR_USER_NAME]@[YOUR_DOMAIN].onmicrosoft.com"
$password = ConvertTo-SecureString "[YOUR_PASSWORD]" -AsPlainText -Force
 

$outfilepath = $siteurl -replace ':', '_' -replace '/', '_'
 
try
{
   [Microsoft.SharePoint.Client.ClientContext]$cc = New-Object Microsoft.SharePoint.Client.ClientContext($siteurl)
   [Microsoft.SharePoint.Client.SharePointOnlineCredentials]$spocreds = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($username, $password)
 
   $cc.Credentials = $spocreds
   Write-Host -ForegroundColor Yellow 'SideLoading feature is not enabled on the site:' $siteurl
   $site = $cc.Site;
 
   $sideLoadingGuid = new-object System.Guid "AE3A1339-61F5-4f8f-81A7-ABD2DA956A7D"
   $site.Features.Add($sideLoadingGuid, $true, [Microsoft.SharePoint.Client.FeatureDefinitionScope]::None);
   $cc.ExecuteQuery();
   Write-Host -ForegroundColor Green 'SideLoading feature enabled on site' $siteurl
 
   #Activate the Developer Site feature
}
catch
{
   Write-Host -ForegroundColor Red 'Error encountered when trying to enable SideLoading feature' $siteurl, ':' $Error[0].ToString();
}
