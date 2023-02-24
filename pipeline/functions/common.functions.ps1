function Random-String {
    param (
        [int] $Length,
        [bool] $numbers = $TRUE,
        [bool] $UpperCase = $FALSE
    )
    $samplechars = (97..122)

    if ($numbers) {
        $samplestring = $samplestring + (48..57)
    }
    if ($UpperCase) {
        $samplestring = $samplestring + (48..57)
    }

    [String]( -join ($samplestring | Get-Random -Count $Length | ForEach-Object { [char]$_ }))
}

function Retry {
    param(
        $Retries = 5,
        $DelayInSeconds = 3,
        $Func
    )

    $lastException = $null
    $count = 0
    do {
        try {
            $Func.Invoke()
            $success = $true
        }
        catch {
            $lastException = $_.Exception
            Write-Host "Operation failed: " $_.Exception.Message
            Write-Output "Retrying in $DelayInSeconds seconds"
            Start-Sleep -Seconds $DelayInSeconds
            # Put the start-sleep in the catch statemtnt so we
            # don't sleep if the condition is true and waste time
        }

        $count++

    }until($count -eq ($Retries + 1) -or $success)
    if (-not($success)) {
        Throw $lastException
    }
}