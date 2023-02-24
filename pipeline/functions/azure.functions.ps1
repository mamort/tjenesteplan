
function ResourceGroup-Exists {
    param(
        [string] $ResourceGroup
    )

    Get-AzResourceGroup -Name $ResourceGroup -ErrorVariable notPresent -ErrorAction SilentlyContinue

    if ($notPresent) {
        $false
    }
    else {
        $true
    }
}

function Create-ResourceGroup {
    param(
        [string] $Name,
        [string] $Location
    )

    if ((ResourceGroup-Exists $Name) -eq $false) {
        New-AzResourceGroup `
            -Name $Name `
            -Location $Location
    }

}