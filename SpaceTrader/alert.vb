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

    Private title As String
    Private text As String
    Private priority As Integer
End Class
