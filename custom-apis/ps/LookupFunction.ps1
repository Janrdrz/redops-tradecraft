function LookupFunc {
    Param ($moduleName, $functionName)
    Write-Host "[*] Looking up $functionName in $moduleName"
    $assem = ([AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.GlobalAssemblyCache -And $_.Location.Split('\\')[-1].Equals('System.dll') }).GetType('Microsoft.Win32.UnsafeNativeMethods')
    $tmp=@();$assem.GetMethods() | ForEach-Object {If($_.Name -eq "GetProcAddress") {$tmp+=$_}}
    $address = $tmp[0].Invoke($null, @(($assem.GetMethod('GetModuleHandle')).Invoke($null,@($moduleName)), $functionName))
    Write-Host "[+] Found $functionName at 0x$($address.ToString('X16'))"
    return $address
}

Write-Host "[*] Locating VirtualProtect in kernel32.dll"
[IntPtr]$funcAddress = LookupFunc kernel32.dll ("VirtualProtect")