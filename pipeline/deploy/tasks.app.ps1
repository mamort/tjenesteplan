. $RootFolder\pipeline\functions\build.functions.ps1
. $RootFolder\pipeline\functions\azure.functions.ps1

# Synopsis:
task PublishFunctionApp Build, {
    $appProjectName = $FunctionApp.AppProjectName
    Publish-App `
        -projectFile "$SourceFolder\$appProjectName\$appProjectName.csproj" `
        -buildConfiguration $Settings.BuildConfiguration `
        -buildFolder $BuildFolder `
        -buildArtifactsFolder $BuildArtifactsFolder `
        -appProjectName $appProjectName
}

task PublishWebApp Build, {
    $appProjectName = $WebApp.AppProjectName
    Publish-App `
        -projectFile "$SourceFolder\$appProjectName\$appProjectName.csproj" `
        -buildConfiguration $Settings.BuildConfiguration `
        -buildFolder $BuildFolder `
        -buildArtifactsFolder $BuildArtifactsFolder `
        -appProjectName $appProjectName
}

# Synopsis:
task ProvisionApp {
    $appProjectName = "app"
    $resourceGroup = $Settings.AppResourceGroup
    $deploymentName = "deploy-" + (Random-String -Length 5)
    Try {

        Create-ResourceGroup `
            -Name $resourceGroup `
            -Location $Settings.AzureLocation

        New-AzResourceGroupDeployment `
            -Name $deploymentName `
            -Mode 'Complete' `
            -TemplateFile "$ArmTemplateFolder/$appProjectName/azuredeploy.json" `
            -TemplateParameterFile "$ArmTemplateFolder/$appProjectName/$Environment.parameters.json" `
            -ResourceGroupName $resourceGroup `
            -environment $Environment `
            -webAppName $WebApp.AppName `
            -websiteBaseUrl $Settings.SiteUrl `
            -functionAppName $FunctionApp.AppName `
            -Force | Out-Null
    }
    catch {
        $_

        throw
    }
}

task ProvisionSecretsKeyvault {
    $resourceGroup = "tjenesteplan-$Environment-secrets-rg"
    $deploymentName = "deploy-" + (Random-String -Length 5)
    Try {

        Create-ResourceGroup `
            -Name $resourceGroup `
            -Location $Settings.AzureLocation

        New-AzResourceGroupDeployment `
            -Name $deploymentName `
            -Mode 'Complete' `
            -TemplateFile "$ArmTemplateFolder/keyvault/azuredeploy.json" `
            -TemplateParameterFile "$ArmTemplateFolder/keyvault/$Environment.parameters.json" `
            -ResourceGroupName $resourceGroup `
            -environment $Environment `
            -webAppName $WebApp.AppName `
            -tjenesteplanDevelopersAADGroupObjectId $Settings.TjenesteplanDevelopersAADGroupObjectId `
            -Force | Out-Null
    }
    catch {
        $_

        throw
    }
}

# Synopsis:
task DeployFunctionApp {
    $appProjectName = $FunctionApp.AppProjectName
    Publish-AzWebApp `
        -ResourceGroupName $Settings.AppResourceGroup `
        -Name $FunctionApp.AppName `
        -ArchivePath $BuildArtifactsFolder\$appProjectName\$appProjectName.zip `
        -Force
}

# Synopsis:
task DeployWebApp DeployStaticWebsite, {
    $appProjectName = $WebApp.AppProjectName
    Publish-AzWebApp `
        -ResourceGroupName $Settings.AppResourceGroup `
        -Name $WebApp.AppName `
        -ArchivePath $BuildArtifactsFolder\$appProjectName\$appProjectName.zip `
        -Force
}

task DeployStaticWebsite {
    $storageAccountName = "tjenesteplan" + $Environment + "sa"
    $storageAccount = Get-AzStorageAccount `
        -ResourceGroupName $Settings.AppResourceGroup `
        -AccountName $storageAccountName

    $storageAccountContext = $storageAccount.Context

    Enable-AzStorageStaticWebsite `
        -Context $storageAccountContext `
        -IndexDocument "index.html" `
        -ErrorDocument404Path "index.html"

    $wwwRootPath = "$BuildArtifactsFolder\wwwroot"

    # Replaces the url to the web api with the actual url for the environment
    $javascriptBundle = "$wwwRootPath\*.bundle.js"
    ((Get-Content -path $javascriptBundle -Raw) `
        -replace 'https://tjenesteplan.placeholder.url', $WebApp.Url) `
        | Set-Content -Path $javascriptBundle

    $mimetypes = @{
        html = "text/html"
        js = "text/javascript"
        css = "text/css"
        svg = "image/svg+xml";
        ttf = "application/x-font-ttf"
        otf = "application/x-font-opentype"
        woff = "application/font-woff"
        woff2 = "application/font-woff2"
        eot = "application/vnd.ms-fontobject"
    }

    $files = Get-ChildItem -Path "$wwwRootPath\*" -Include "*" -Recurse | Select-Object FullName,Extension

    # Relative-Path function resolves path to current dir. This temporarily sets current dir to wwwRoot-path
    Push-Location $wwwRootPath
    foreach($file in $files) {
        $ext = $file.Extension.Substring(1, $file.Extension.Length-1)
        $mime = $mimetypes[$ext]

        $relativePath = Resolve-Path $file.FullName -Relative

        if(!$mime) {
            $mime = "application/octet-stream"
        }

        $cacheControl = "public, max-age=86400"
        if($ext -eq "html") {
            $cacheControl = "public, no-cache, must-revalidate"
        }

        Set-AzStorageBlobContent `
            -Container '$web' `
            -File $relativePath `
            -Blob $relativePath `
            -Context $StorageAccountContext `
            -Properties @{
                "ContentType" = $mime;
                "CacheControl" = "public, no-cache, must-revalidate"
            } `
            -Force | Out-Null
    }
    Pop-Location

    Retry `
        -Func {
            Set-AzStorageAccount `
                -ResourceGroupName $Settings.AppResourceGroup `
                -AccountName $storageAccountName `
                -CustomDomainName $Settings.DomainName `
                -UseSubDomain $True
        }
}

function UploadWebsiteAssets {
    param(
        $Path,
        $IncludePattern,
        $StorageAccountContext,
        $ContentType
    )

    Get-ChildItem -Path "$Path\*" -Include $IncludePattern -Recurse | `
        Set-AzStorageBlobContent `
            -Container '$web' `
            -Context $StorageAccountContext `
            -Properties @{
                "ContentType" = $ContentType;
                "CacheControl" = "public, no-cache, must-revalidate"
            } `
            -Force | Out-Null
}