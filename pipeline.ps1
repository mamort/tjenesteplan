Param(
    [string] $Target = "?",
    [string] $Environment = "dev"
)

$script:Environment = $Environment
$script:NoRestore = $NoRestore
$script:RootFolder = "$PSScriptRoot"
$script:FrontendSourceFolder = "$RootFolder\src\frontend"
$script:SourceFolder = "$RootFolder\src\backend"
$script:BuildFolder = "$RootFolder\build"
$script:BuildArtifactsFolder = "$RootFolder\artifacts"
$script:ArmTemplateFolder = "$RootFolder\pipeline\deploy\arm-templates"

. "$RootFolder\pipeline\settings\settings.ps1"
. "$RootFolder\pipeline\settings\settings.$($Environment).ps1"

$InvokeBuildVersion = "5.5.1"
$InvokeBuildFolder = "$RootFolder\pipeline\build\Invoke-Build.$InvokeBuildVersion"

if (!(Test-Path $InvokeBuildFolder)) {
    & "$RootFolder\.nuget\nuget.exe" `
        "install" "Invoke-Build" `
        "-Version" $InvokeBuildVersion `
        "-Source" "https://api.nuget.org/v3/index.json" `
        "-OutputDirectory" "$RootFolder\pipeline\build"
}

Import-Module $InvokeBuildFolder\tools\InvokeBuild.psd1

try {
    Invoke-Build $Target $RootFolder\pipeline\tasks.pipeline.ps1 -Result Result
}
finally {
    # Show task summary information after the build
    $Result.Tasks | Format-Table Elapsed, Name, Error -AutoSize
}
