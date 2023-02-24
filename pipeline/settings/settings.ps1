
$projectResourceNamePrefix = "tjenesteplan-$Environment"
$script:Settings = @{
    ProjectResourceNamePrefix = $projectResourceNamePrefix
    BuildConfiguration = "release"
    BuildVersion = "1.0.0.0"
    AzureLocation  = "WestEurope"
    AppResourceGroup = "$projectResourceNamePrefix-rg"
}

$script:FunctionApp = @{
    AppProjectName = "Tjenesteplan.FunctionApp"
    AppName        = "tjenesteplan-$Environment-func"
}

$script:WebApp = @{
    AppProjectName = "Tjenesteplan.Api"
    AppName        = "tjenesteplan-$Environment-webapi"
}

$WebApp.Url = "https://$($WebApp.AppName).azurewebsites.net/api"