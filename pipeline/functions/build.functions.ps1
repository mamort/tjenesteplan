function Publish-App {
    param (
        $projectFile,
        $buildConfiguration,
        $buildFolder,
        $buildArtifactsFolder,
        $appProjectName
    )

    dotnet publish $projectFile `
        --configuration $buildConfiguration `
        --output $buildFolder\$appProjectName

    New-Item -ItemType Directory -Force -Path $buildArtifactsFolder\$appProjectName

    Compress-Archive `
        -Path $buildFolder\$appProjectName\* `
        -Force `
        -CompressionLevel Fastest `
        -DestinationPath $buildArtifactsFolder\$appProjectName\$appProjectName.zip

    Get-ChildItem $buildArtifactsFolder\$appProjectName\* `
        -Exclude *.zip `
        -Recurse `
        -Force | Remove-Item -Recurse -Force
}