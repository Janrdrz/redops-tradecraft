function delegateType {
    param([Type[]]$parameters,[Type]$returnType = [Void])
    Write-Host "[*] Creating delegate type for function pointer" 
    $type = [AppDomain]::CurrentDomain.DefineDynamicAssembly((New-Object Reflection.AssemblyName('ReflectedDelegate')),[Reflection.Emit.AssemblyBuilderAccess]::Run).DefineDynamicModule('InMemoryModule',$false).DefineType('MyDelegateType', 'Class, Public, Sealed, AnsiClass, AutoClass',[MulticastDelegate])
    $type.DefineConstructor('RTSpecialName, HideBySig, Public',[Reflection.CallingConventions]::Standard,$parameters).SetImplementationFlags('Runtime, Managed')
    $type.DefineMethod('Invoke', 'Public, HideBySig, NewSlot, Virtual',$returnType, $parameters).SetImplementationFlags('Runtime, Managed')
    return $type.CreateType()
}
