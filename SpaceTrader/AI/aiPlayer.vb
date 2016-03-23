Public Class aiPlayer
    Inherits player
    Friend Shared Function build(ByVal tier As Integer) As aiPlayer
        Dim player As New aiPlayer
        With player
            Select Case tier
                Case 1
                    .name = "Independent Pirates"
                    aiShip.build(player, eShipType.Corvette)

                Case 2
                    .name = "Freelance Corsairs"
                    If coinFlip() = True Then
                        'corvette x3
                        For n = 1 To 3
                            aiShip.build(player, eShipType.Corvette)
                        Next
                    Else
                        'frigate x1
                        aiShip.build(player, eShipType.Frigate)
                    End If

                Case 3
                    .name = "Raider Gangs"
                    Select Case rng.Next(1, 11)
                        Case 1 To 4
                            'frigate x3

                        Case 5 To 8
                        Case 9 To 10
                    End Select

                Case 4
                    .name = "Galactic Syndicate"

                Case Else
                    MsgBox("aiPlayer.buildRandom: tier out of bound.")
                    Return Nothing
            End Select
        End With
        Return player
    End Function
End Class
