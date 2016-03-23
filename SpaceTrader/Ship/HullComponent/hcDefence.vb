Public Class hcDefence
    Inherits hullComponent
    Implements ihcCrewable
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aType As eDefenceType, ByVal aValue As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        _defType = aType
        _value = aValue
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return withSign(value) & " " & defType.ToString
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

    Friend pdEnergyCost As Integer
    Friend Sub pdAttack(ByVal target As interceptor)
        If defType <> eDefenceType.PointDefence Then Exit Sub
        If crewable.isManned = False Then
            Console.WriteLine(name & " is not crewed!")
            Console.ReadKey()
            Exit Sub
        End If
        If ship.addEnergyCheck(-pdEnergyCost) = False Then
            Console.WriteLine("Insufficient energy!")
            Console.ReadKey()
            Exit Sub
        End If
        If useResource() = False Then Exit Sub

        ship.addEnergy(-pdEnergyCost)
        ship.player.addAlert("Point Defence", name & " destroys a " & target.name & ".", 2)
        target.destroy()
    End Sub

    Friend Property crewable As New shcCrewable(Me) Implements ihcCrewable.crewable
    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Defence"
        End Get
    End Property
End Class
