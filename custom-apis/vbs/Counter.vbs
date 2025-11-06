Function Counter()
    Dim i As Integer
    Dim j As Long
    Dim k As Double
    
    For i = 1 To 10
        For j = 0 To 5000000 - 1
            k = Sqr(j)
        Next j
        MsgBox ("Counting: " & i)
    Next i
End Function
