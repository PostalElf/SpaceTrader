Public Class hcDefence
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aType As eDefenceType, ByVal aValue As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0, Optional ByVal aPdEnergyCost As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        _defType = aType
        _value = aValue
        _energyCost = aPdEnergyCost
    End Sub
    Friend Overrides Function consoleDescription() As String
        Dim def As String
        If defType = eDefenceType.PointDefence Then def = "Point Defence" Else def = defType.ToString
        Return withSign(value) & " " & def
    End Function
    Friend Overrides ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)(MyBase.alarms)
            If crewable.isManned = False Then total.Add("Requires crew member(s).")
            Return total
        End Get
    End Property

    Private _defType As eShipType
    Friend ReadOnly Property defType As eDefenceType
        Get
            Return _defType
        End Get
    End Property
    Private _value As Integer
    Friend ReadOnly Property value As Integer
        Get
            If crewable.isManned = True Then Return _value Else Return 0
        End Get
    End Property

    Friend Overrides Sub tickTravel()
        tickRechargeShields()
    End Sub
    Friend Overrides Sub tickIdle()
        tickRechargeShields()
    End Sub
    Private Sub tickRechargeShields()
        If defType = eDefenceType.Shields Then
            Dim shieldsRaw As Integer() = ship.getDefences(eDefenceType.Shields)
            Dim shields As Integer = shieldsRaw(0)
            Dim shieldsMax As Integer = shieldsRaw(1)
            If shields < shieldsMax Then
                If crewable.isManned = False Then Exit Sub
                If useResource() = False Then Exit Sub
                ship.repair(eDefenceType.Shields, 1)
            End If
        End If
    End Sub

    Friend Overrides Function UseCombat(ByRef target As iCombatant) As Boolean
        If MyBase.UseCombat(target) = False Then Return False
        If TypeOf target Is interceptor = False Then Return False

        ship.player.addAlert("Point Defence", ship.name & "'s " & name & " destroys a " & target.name & ".", 2)
        target.addAlert("Point Defence", ship.name & "'s " & name & " destroys a " & target.name & ".", 2)
        target.destroy()
        Return True
    End Function
    Friend Overrides Function UseCombatCheck(ByRef target As iCombatant) As Boolean
        If MyBase.UseCombatCheck(target) = False Then Return False
        If TypeOf target Is interceptor = False Then Return False

        Return True
    End Function

    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Defence"
        End Get
    End Property
End Class
