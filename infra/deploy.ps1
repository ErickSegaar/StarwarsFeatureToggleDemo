[CmdletBinding()]
param(
    $ResourceGroupName,
    $ResourceGroupLocation
)

$configurationName = "StarwarsConfiguration"
#az login
#az account set --subscription $SubscriptionId

az extension add -n appconfig

#Create the resource group
az group create `
    --name $ResourceGroupName `
    --location $ResourceGroupLocation

az appconfig create --location $ResourceGroupLocation `
    --name $configurationName `
    --resource-group $ResourceGroupName

az appconfig kv set `
    --name $configurationName `
    --key .appconfig.featureflag/ShowPlanetsOfStarwars `
    --value '"{\"id\":\"ShowPlanetsOfStarwars\",\"description\":\"'When enabled the planets view will be used to show planets from the swapi api'\",\"enabled\":false,\"label\":\"\",\"conditions\":{\"client_filters\":[]}}"' `
    --content-type "application/vnd.microsoft.appconfig.ff+json;charset=utf-8" `
    --yes

az appconfig kv set `
    --name $configurationName `
    --key .appconfig.featureflag/UseImprovedStarshipProvider `
    --value '"{\"id\":\"UseImprovedStarshipProvider\",\"description\":\"'When enabled the new improved client to swapi.io will be used, when disabled the incomplete old hardconfigured data will be used'\",\"enabled\":true,\"label\":null,\"conditions\":{\"client_filters\":[{\"name\":\"Microsoft.Percentage\",\"parameters\":{\"value\":\"50\"}}]}}"' `
    --content-type "application/vnd.microsoft.appconfig.ff+json;charset=utf-8" `
    --yes

# Get the AppConfig primary key 
$connectionString = ConvertFrom-Json $( $(az appconfig credential list --name StarwarsConfiguration -o json --query "[?name=='Primary Read Only'].{ConnectionString:connectionString}") -join '')

Write-Output "Connection string to use: $connectionString"