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
        _isActive = False
    End Sub

    Private _isActive As Boolean = True
    Friend ReadOnly Property isActive As Boolean
        Get
            Return _isActive
        End Get
    End Property
    Friend ships As New Dictionary(Of player, List(Of ship))
    Friend Function getEnemyShips(ByRef player As player) As List(Of ship)
        Dim total As New List(Of ship)
        For Each kvp In ships
            If kvp.Key.Equals(player) = False Then total.AddRange(kvp.Value)
        Next
        Return total
    End Function
    Friend Function getEnemyShipRandom(ByRef player As player) As ship
        Dim enemies As List(Of ship) = getEnemyShips(player)
        If enemies.Count = 0 Then Return Nothing

        Dim roll As Integer = rng.Next(enemies.Count)
        Return enemies(roll)
    End Function
    Friend Sub tickCombat()
        For Each kvp In ships
            For n = kvp.Value.Count - 1 To 0 Step -1
                Dim ship As ship = kvp.Value(n)
                ship.tickCombat()
            Next
        Next

        'remove players with no ships remaining
        For n = ships.Keys.Count - 1 To 0 Step -1
            Dim key As player = ships.Keys(n)
            If ships(key).Count = 0 Then ships.Remove(key)
        Next

        If ships.Keys.Count = 1 Then
            Close()
        End If
    End Sub
End Class
