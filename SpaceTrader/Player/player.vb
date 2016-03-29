Public Class player
    Friend name As String
    Private _credits As Integer
    Friend ReadOnly Property credits As Integer
        Get
            Return _credits
        End Get
    End Property
    Friend Sub addCredits(ByVal value As Integer)
        _credits += value
    End Sub
    Friend Function addCreditsCheck(ByVal value As Integer) As Boolean
        If value > _credits Then Return False
        Return True
    End Function

    Private alerts As New SortedList(Of Integer, List(Of alert))
    Friend Sub addAlert(ByVal title As String, ByVal text As String, ByVal priority As Integer)
        Dim alert As New alert(title, text, priority)
        If alerts.ContainsKey(priority) = False Then alerts.Add(priority, New List(Of alert))
        alerts(priority).Add(alert)
    End Sub
    Friend Sub alertsClear()
        alerts.Clear()
    End Sub
    Friend Sub alertsConsoleReport(ByVal indent As Integer, Optional ByVal maxPriority As Integer = 999)
        If alerts.Count > 0 Then
            For Each kvp In alerts
                If kvp.Key <= maxPriority Then
                    Dim alertList As List(Of alert) = kvp.Value
                    For Each alert In alertList
                        alert.consoleReport(indent)
                    Next
                    If alertList.Count > 0 Then Console.WriteLine()
                End If
            Next
        End If
        alerts.Clear()
    End Sub

    Friend ships As New List(Of ship)

    Private questVariables As New Dictionary(Of String, Integer)
    Friend Sub addQuestVariable(ByVal var As String, ByVal value As Integer)
        If questVariables.ContainsKey(var) = False Then questVariables.Add(var, 0)
        questVariables(var) += value
    End Sub
    Friend Sub setQuestVariable(ByVal var As String, ByVal value As Integer)
        If questVariables.ContainsKey(var) = False Then questVariables.Add(var, 0)
        questVariables(var) = value
    End Sub
    Friend Function checkQuestVariable(ByVal var As String, ByVal range As range) As Boolean
        If questVariables.ContainsKey(var) = False Then
            If range.min = 0 Then Return True Else Return False
        End If

        Return range.isWithin(questVariables(var))
    End Function
End Class
