Public Class battlefield
    Friend Sub New(ByVal players As List(Of player))
        For Each player In players
            ships.Add(player, New List(Of ship))
            ships(player).AddRange(player.ships)

            For Each ship In player.ships
                ship.enterCombat(Me)
            Next
        Next
    End Sub
    Friend Sub Close()
        For Each kvp In ships
            For Each ship In kvp.Value
                ship.leaveCombat()
            Next
        Next
    End Sub

    Friend ships As New Dictionary(Of player, List(Of ship))
    Friend Sub tickCombat()
        For Each kvp In ships
            For Each ship In kvp.Value
                ship.tickCombat()
            Next
        Next
    End Sub
End Class
