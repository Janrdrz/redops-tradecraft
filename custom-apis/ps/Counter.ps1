for ($i = 1; $i -le 10; $i++) {
    for ($j = 0; $j -lt 5000000; $j++) {
        [Math]::Sqrt($j) | Out-Null
    }
    Write-Host "Counting: $i"
}