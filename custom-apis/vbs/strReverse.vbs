Function Strings(strArg As String)

    Dim lengthtotal, letter, res
    total_length = Len(strArg)
    
    For letter = 1 To total_length
         res = Mid(strArg, letter, 1) & res
    Next
    
    MsgBox "Reversed String is: " & res
    Strings = res

End Function