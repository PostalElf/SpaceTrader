Public Class quest
    Private name As String
    Private text As String
    Private travelPhase As String
    Private removeAfterUse As Boolean = False
    Private questVariables As New Dictionary(Of String, range)
    Private questChoices As New List(Of questChoice)

    Private Shared questDeck As New List(Of quest)
    Friend Shared Function drawRandom(ByRef player As player) As quest
        If questDeck.Count = 0 Then buildQuestDeck()

        Dim realDeck As New List(Of quest)
        For Each quest In questDeck
            If quest.isLegal(player) Then realDeck.Add(quest)
        Next
        If realDeck.Count = 0 Then Return Nothing

        Dim roll As Integer = rng.Next(realDeck.Count)
        Dim choice As quest = realDeck(roll)
        If choice.removeAfterUse = True Then questDeck.Remove(choice)
        Return choice
    End Function
    Private Shared Sub buildQuestDeck()
        Dim data As List(Of Queue(Of String)) = bracketFilegetAll("data/quests.txt")
        For Each q In data
            Dim quest As New quest
            With quest
                .name = q.Dequeue
                While q.Count > 0
                    Dim l As String = q.Dequeue
                    Dim ln As String() = l.Split(":")
                    For n = 0 To ln.Count - 1
                        ln(n) = ln(n).Trim
                    Next

                    Select Case ln(0).ToLower
                        Case "text" : .text = ln(1)
                        Case "travelphase" : .travelPhase = ln(1)
                        Case "removeafteruse" : .removeAfterUse = True
                        Case "variables"
                            'variables: Doggy 1-1, Puppy 1-3
                            Dim raw As String() = ln(1).Split(",")
                            For n = 0 To raw.Count - 1
                                raw(n) = raw(n).Trim
                            Next
                            For Each rawpart In raw
                                Dim rawsplit As String() = rawpart.Split(" ")
                                Dim var As String = rawsplit(0)
                                Dim values As String() = rawsplit(1).Split("-")
                                Dim min As Integer = CInt(values(0))
                                Dim max As Integer = CInt(values(1))
                                .questVariables.Add(var, New range(min, max))
                            Next
                        Case "choice"
                            .questChoices.Add(questChoice.build(q))
                    End Select
                End While
            End With
            questDeck.Add(quest)
        Next
    End Sub

    Friend ReadOnly Property isLegal(ByVal player As player) As Boolean
        Get
            For Each qv In questVariables
                If player.checkQuestVariable(qv.Key, qv.Value) = False Then Return False
            Next
            If travelPhase <> "" Then
                For Each ship In player.ships
                    If ship.travelPhase <> travelPhase Then Return False
                Next
            End If

            Return True
        End Get
    End Property
End Class
