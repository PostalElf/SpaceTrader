Public Class alert
    Public Sub New(ByVal aTitle As String, ByVal aText As String, ByVal aPriority As Integer)
        title = aTitle
        text = aText
        priority = aPriority
    End Sub

    Private title As String
    Private text As String
    Private priority As Integer
    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Console.WriteLine(ind & text)
    End Sub
End Class
