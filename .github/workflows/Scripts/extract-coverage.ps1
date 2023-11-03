param(
    [string]$coverageThreshold = "0.7",
    [string]$coverageXml = "merged.cobertura.xml"
)

if (Test-Path $coverageXml) {
    [xml]$coverageData = Get-Content $coverageXml
    $actualCoverage = $coverageData.coverage.'line-rate'
    if ($actualCoverage -lt $coverageThreshold) {
        Write-Host "Code coverage is below the threshold. Coverage: $actualCoverage%"
        exit 1
    }
    Write-Host "Code coverage is within the threshold. Coverage: $actualCoverage%"
} else {
    Write-Host "Code coverage file not found."
    exit 1
}
