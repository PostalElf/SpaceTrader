Public Class alert
    Private Shared alerts As New SortedList(Of Integer, List(Of alert))
    Friend Shared Sub Add(ByVal title As String, ByVal text As String, ByVal priority As Integer)
        Dim alert As New alert
        With alert
            .title = title
            .text = text
            .priority = priority

            If alerts.ContainsKey(.priority) = False Then alerts.Add(.priority, New List(Of alert))
            alerts(.priority).Add(alert)
        End With
    End Sub
    Friend Shared Sub allConsoleReport(ByVal indent As Integer)
        If alerts.Count > 0 Then
            For Each kvp In alerts
                Dim alertList As List(Of alert) = kvp.Value
                For Each alert In alertList
                    alert.consoleReport(indent)
                Next
            Next
        End If
        alerts.Clear()
    End Sub

    Private title As String
    Private text As String
    Private priority As Integer
    Private Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Console.WriteLine(ind & text)
    End Sub
End Class
