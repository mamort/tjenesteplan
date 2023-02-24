# Synopsis: Clean build artifact folder
task Clean {
    if (Test-Path $BuildArtifactsFolder) {
        Get-ChildItem -Path "$BuildArtifactsFolder\*" -Recurse | Remove-Item -Force -Recurse
        Remove-Item "$BuildArtifactsFolder\*" -Force
    }

    if (Test-Path $BuildFolder) {
        Get-ChildItem -Path "$BuildFolder\*" -Recurse | Remove-Item -Force -Recurse
        Remove-Item "$BuildFolder\*" -Force
    }
}

# Synopsis: Build all projects
task BuildFrontend Clean, {
    cd $FrontendSourceFolder
    & npm install
    # Need to use a replacement value here so that we during deploy
    # can replace this value with the actual url for the environment
    $env:REACT_APP_API_URL = "https://tjenesteplan.placeholder.url"
    & npm run build
    Remove-Item Env:\REACT_APP_API_URL
    cd $RootFolder

    Copy-Item `
        -Path "$BuildFolder\wwwroot" `
        -Destination "$BuildArtifactsFolder\wwwroot" `
        -Recurse
}

# Synopsis: Build all projects
task BuildBackend Clean, {
    write-host $Settings.BuildVersion
    $files = Get-ChildItem -path "$SourceFolder" -Include *.csproj -Recurse
    $files | foreach-object {
        dotnet build $_.FullName `
            /property:Version=$($Settings.BuildVersion) `
            --configuration $($Settings.BuildConfiguration)
    }
}

# Synopsis: Build all projects
task Build BuildFrontend, BuildBackend

# Synopsis: Build solution and run all test project
task Test Build, UnitTests

# Synopsis: Run all tests
task UnitTests {
    if (Test-Path "$SourceFolder\*Tests") {
        $files = Get-ChildItem -Path "$SourceFolder\*Tests" -Include *.csproj -Recurse
        $files | foreach-object {
            $testLogger = "trx;LogFileName=$RootFolder\testresults\$($_.Name).testresults.trx"
            Write-Host $testLogger
            dotnet test $_.FullName --logger $testLogger --configuration $Settings.BuildConfiguration
        }
    }
}
