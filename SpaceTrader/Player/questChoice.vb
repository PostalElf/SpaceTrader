Public Class questChoice
    Friend Shared Function build(ByRef q As Queue(Of String)) As questChoice
        Dim total As New questChoice
        With total
            While q.Count > 0 AndAlso (q.Peek.ToLower <> "outcome:" AndAlso q.Peek.StartsWith(";") = False)
                Dim l As String = q.Dequeue
                Dim ln As String() = l.Split(":")
                For n = 0 To ln.Count - 1
                    ln(n) = ln(n).Trim
                Next

                Select Case ln(0).ToLower
                    Case "text"
                        .text = ln(1)

                    Case "effect"
                        Dim raw As String() = ln(1).Split(",")
                        For Each rawpart In raw
                            .effects.Add(rawpart.Trim)
                        Next

                    Case "result"
                        .result = ln(1)
                End Select
            End While
        End With
        Return total
    End Function

    Private text As String
    Private effects As New List(Of String)
    Private result As String

    Friend Sub applyEffects(ByRef player As player)
        For Each effect In effects
            Dim ln As String() = effect.Split(" ")
            Select Case ln(0).ToLower
                Case "damagearmour"
                    Dim value As Integer = CInt(ln(1))
                    For Each ship In player.ships
                        ship.addDamageArmour(value)
                    Next
                Case "damageshields"
                    Dim value As Integer = CInt(ln(1))
                    For Each ship In player.ships
                        ship.addDamageShields(value)
                    Next
                Case "goods"
                    Dim goodType As eResource = constants.getEnumFromString(ln(1), constants.resourceArray)
                    Dim value As Integer = CInt(ln(2))
                    For Each ship In player.ships
                        ship.addResource(goodType, value)
                    Next
                Case Else
                    If ln(1).StartsWith("=") Then
                        Dim value As Integer = CInt(ln(1).Remove(0, 1))
                        player.setQuestVariable(ln(0), value)
                    Else
                        Dim value As Integer = CInt(ln(1))
                        player.addQuestVariable(ln(0), value)
                    End If
            End Select
        Next
    End Sub
End Class
