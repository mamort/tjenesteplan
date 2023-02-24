. $PSScriptRoot\functions\common.functions.ps1

# Imports the build and deploy tasks
. $PSScriptRoot\build\tasks.build.ps1
. $PSScriptRoot\deploy\tasks.app.ps1

# Synopsis: Creates all artifacts needed to deploy all apps
task BuildArtifacts PublishWebApp, PublishFunctionApp

# Synopsis: Provision infrastructure for all apps
task Provision ProvisionApp, ProvisionSecretsKeyvault

# Synopsis: Deploy all apps
task Deploy Provision, DeployFunctionApp, DeployWebApp

# Synopsis: Build, provision and deploy all apps
task . BuildArtifacts, Deploy
