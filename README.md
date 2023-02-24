# Based on the following tutorial
http://jasonwatmore.com/post/2017/09/16/react-redux-user-registration-and-login-tutorial-example
https://www.pointblankdevelopment.com.au/blog/135/react-redux-with-aspnet-core-20-login-registration-tutorial-example

# Getting Started
Install .NET Core 2.1 SDK (v2.1.300) or higher. This is the version that introduced global tools.

# Build
Powershell is used together with the Invoke-Build library for build & deploy.

The AzureRm Powershell module has been replaced by the Az-module. It's best to use it with Powershell Core 6.

Download Powershell Core 6:
https://github.com/PowerShell/PowerShell/releases/tag/v6.2.2

Then open Powershell Core shell and install Az:
Install-Module -Name Az -AllowClobber

```powershell
.\pipeline.ps1 -Target Build -Environment dev
```
Or shorthand version:
```powershell
.\pipeline.ps1 Build
```
Without any parameters will list all available targets
```powershell
.\pipeline.ps1
```

# Frontend
- Navigate to src/frontend
- run "npm install"
- run "npm start"

Frontend will be available on url: http://localhost:8080

# Backend
- Navigate to src/backend/Tjenesteplan.Api
- run "dotnet run"

Backend can also be run from Visual Studio and will be available at: http://localhost:5000

# Database
- MS SQLExpress needs to be installed
- look in appsettings.development.json to see details for the local SQL server username and password
- run the following SQL to add login for tjenesteplan:
```sql
CREATE LOGIN "tjenesteplan" WITH PASSWORD='<password goes here>'
GO
CREATE USER "tjenesteplan" FOR LOGIN "tjenesteplan"  
GO 
EXECUTE sys.sp_addsrvrolemember
    @loginame = N'tjenesteplan',
    @rolename = N'dbcreator'
GO
```
# Migrations
EF Core Migrations Code First is used.
When you want to change something in the database add your changes in Tjenesteplan.Data under one or more of the features.
As you are ready with your new or modified entities, use EF Core migrations to generate the migrations.
Using command line execute the following command from Tjenesteplan.Data folder:
```powershell
dotnet ef --startup-project ../Tjenesteplan.Api migrations add <name of your migration>
```

# Select correct Azure tenant locally
Connect-AzAccount
Select-AzSubscription -TenantId eb9d3374-8e34-4284-a292-6cc898962d4f