Public Class ship
    Public Sub New()
        For Each r In constants.resourceArray
            resources.Add(r, 0)
            resourcesMax.Add(r, 100)
        Next
    End Sub
    Friend Shared Function build(ByRef player As player, ByVal type As eShipType) As ship
        Dim ship As New ship
        With ship
            ._name = getRandomAndRemove(shipNames, "data/shipnames.txt")
            .player = player
            .type = type
            .buildDefaultShip()
            .fullRepair()
        End With
        Return ship
    End Function
    Private Sub buildDefaultShip()
        For Each r In constants.resourceArray
            resourcesMax(r) = 10
        Next


        Select Case type
            Case eShipType.Corvette
                hullSpaceMax = 20
                damageShieldsMax = 5
                damageArmourMax = 10
            Case eShipType.Frigate
                hullSpaceMax = 50
                damageShieldsMax = 10
                damageArmourMax = 10
            Case eShipType.Crusier
                hullSpaceMax = 75
                damageShieldsMax = 20
                damageArmourMax = 10
            Case eShipType.Destroyer
                hullSpaceMax = 100
                damageShieldsMax = 20
                damageArmourMax = 20
            Case eShipType.Dreadnought
                hullSpaceMax = 150
                damageShieldsMax = 20
                damageArmourMax = 40
        End Select
    End Sub
    Private Shared shipNames As New List(Of String)

    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftlen As Integer = 10

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Credits:", ftlen) & "¥" & player.credits.ToString("N0"))
        Console.WriteLine(indd & fakeTab("Shields:", ftlen) & damageShields & "/" & damageShieldsMax)
        Console.WriteLine(indd & fakeTab("Armour:", ftlen) & damageArmour & "/" & damageArmourMax)
        Console.WriteLine(indd & fakeTab("Hull:", ftlen) & hullSpaceOccupied & "/" & hullSpaceMax)
        For Each hc In hullComponents
            hc.consoleReport(indent + 2, "└ ")
        Next
        Console.WriteLine(indd & "Cargo Bay:")
        For Each r In constants.resourceArray
            Console.WriteLine(inddd & "└ " & fakeTab(r.ToString & ":", 13) & resources(r) & "/" & resourcesMax(r))
        Next
    End Sub

    Private _name As String
    Friend ReadOnly Property name As String
        Get
            Return getTypePrefix() & " " & _name
        End Get
    End Property
    Private player As player
    Private type As eShipType
    Private Function getTypePrefix() As String
        Select Case type
            Case eShipType.Corvette : Return "Cv."
            Case eShipType.Frigate : Return "Fr."
            Case eShipType.Crusier : Return "Cr."
            Case eShipType.Destroyer : Return "De."
            Case eShipType.Dreadnought : Return "Dr."
            Case Else
                MsgBox("getTypePrefix: type not recognised.")
                Return Nothing
        End Select
    End Function

    Private mustRefresh As Boolean = False
    Private Sub refresh()
        'reset all variables
        buildDefaultShip()


        'iterate through hullComponents
        For Each hc In hullComponents
            If TypeOf hc Is hcDefence Then
                Dim d As hcDefence = CType(hc, hcDefence)
                If d.type = eDefenceType.Armour Then damageArmourMax += d.value
                If d.type = eDefenceType.Shields Then damageShieldsMax += d.value
            ElseIf TypeOf hc Is hcCargo Then
                Dim c As hcCargo = CType(hc, hcCargo)
                resourcesMax(c.resource) += c.value
            End If
        Next


        'flag
        mustRefresh = False
    End Sub

    Private damageShields As Integer
    Private damageShieldsMax As Integer
    Private damageArmour As Integer
    Private damageArmourMax As Integer
    Friend Sub fullRepair()
        damageShields = damageShieldsMax
        damageArmour = damageArmourMax
    End Sub
    Friend Sub addDamage(ByVal damage As Integer, ByVal damageType As eDamageType)
        If damageShields > 0 Then
            damageShields -= damage
            If damageShields < 0 Then
                damage = damageShields * -1
                damageShields = 0
                alert.Add("Shields Down", name & "'s shields are down.", 2)
            Else
                alert.Add("Shields", name & " has " & damageShields & " shields remaining.", 2)
            End If
        End If
        If damage > 0 Then
            damageArmour -= damage
            If damageArmour <= 0 Then destroy() Else alert.Add("Armour", name & " has " & damageArmour & " armour remaining.", 2)
        End If
    End Sub
    Private Sub destroy()
        alert.Add("Ship Destruction", _name & " was destroyed!", 0)
    End Sub

    Private hullComponents As New List(Of hullComponent)
    Private hullSpaceMax As Integer
    Private ReadOnly Property hullSpaceOccupied As Integer
        Get
            Dim total As Integer = 0
            For Each hc In hullComponents
                total += hc.size
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property hullSpaceEmpty As Integer
        Get
            Return hullSpaceMax - hullSpaceOccupied
        End Get
    End Property
    Friend Sub addComponent(ByRef hc As hullComponent)
        hullComponents.Add(hc)
        hc.ship = Me
        mustRefresh = True
    End Sub
    Friend Sub removeComponent(ByRef hc As hullComponent)
        If hullComponents.Contains(hc) = False Then Exit Sub

        hc.ship = Nothing
        hullComponents.Remove(hc)
        mustRefresh = True
    End Sub

    Private resources As New Dictionary(Of eResource, Integer)
    Private resourcesMax As New Dictionary(Of eResource, Integer)
    Friend Function addResourceCheck(ByVal resource As eResource, ByVal value As Integer) As Boolean
        If resources(resource) + value > resourcesMax(resource) Then Return False
        If resources(resource) + value < 0 Then Return False

        Return True
    End Function
    Friend Sub addResource(ByVal resource As eResource, ByVal value As Integer)
        resources(resource) += value
    End Sub
End Class
