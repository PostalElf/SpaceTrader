Public Class ship
    Public Sub New()
        For Each r In constants.resourceArray
            resources.Add(r, 0)
            resourcesMax.Add(r, 100)
        Next
        For Each t As Type In constants.hcTypeList
            hullComponents.Add(t, New List(Of hullComponent))
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
                shieldsMax = 5
                armourMax = 10
            Case eShipType.Frigate
                hullSpaceMax = 50
                shieldsMax = 10
                armourMax = 10
            Case eShipType.Crusier
                hullSpaceMax = 75
                shieldsMax = 20
                armourMax = 10
            Case eShipType.Destroyer
                hullSpaceMax = 100
                shieldsMax = 20
                armourMax = 20
            Case eShipType.Dreadnought
                hullSpaceMax = 150
                shieldsMax = 20
                armourMax = 40
        End Select
    End Sub
    Private Shared shipNames As New List(Of String)

    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftlen As Integer = 10

        If mustRefresh = True Then refresh()

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Credits:", ftlen) & "¥" & player.credits.ToString("N0"))
        Console.WriteLine(indd & fakeTab("Shields:", ftlen) & shields & "/" & shieldsMax)
        Console.WriteLine(indd & fakeTab("Armour:", ftlen) & armour & "/" & armourMax)
        Console.Write(indd & fakeTab("Speed:", ftlen) & travelSpeed & " ")
        If travelByJump = True Then Console.WriteLine("jump") Else Console.WriteLine("sublight")

        Console.WriteLine(indd & fakeTab("Hull:", ftlen) & hullSpaceOccupied & "/" & hullSpaceMax)
        consoleReportHullComponents(indent + 2, "└ ")

        Dim crewList As List(Of crew) = getCrew()
        If crewList.Count > 0 Then
            Console.WriteLine(indd & "Crew:")
            For Each crew In crewList
                Console.WriteLine(inddd & "└ " & crew.name)
            Next
        End If

        Console.WriteLine(indd & "Cargo Bay:")
        For Each r In constants.resourceArray
            Console.WriteLine(inddd & "└ " & fakeTab(r.ToString & ":", 13) & resources(r) & "/" & resourcesMax(r))
        Next
    End Sub
    Private Sub consoleReportHullComponents(ByVal indent As Integer, Optional ByVal prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix

        Dim ftlen As Integer = 0
        For Each kvp In hullComponents
            For Each hc In kvp.Value
                If hc.name.Length > ftlen Then ftlen = hc.name.Length
            Next
        Next
        ftlen += 3

        For Each kvp In hullComponents
            For Each hc In kvp.Value
                Console.WriteLine(ind & fakeTab(hc.name & ":", ftlen) & hc.consoleDescription & " " & hc.consoleResourceDescription)
            Next
        Next
    End Sub
    Private mustRefresh As Boolean = False
    Private Sub refresh()
        'reset all variables
        buildDefaultShip()


        'iterate through hullComponents
        For Each hc In hullComponents(GetType(hcDefence))
            Dim d As hcDefence = CType(hc, hcDefence)
            If d.type = eDefenceType.Armour Then armourMax += d.value
            If d.type = eDefenceType.Shields Then shieldsMax += d.value
        Next
        For Each hc In hullComponents(GetType(hcCargo))
            Dim c As hcCargo = CType(hc, hcCargo)
            resourcesMax(c.resource) += c.value
        Next

        'flag
        mustRefresh = False
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

    Private _travelByJump As Boolean = True
    Friend ReadOnly Property travelSpeed As Integer
        Get
            Dim totalSpeed As Integer = 0
            If travelByJump = True Then
                For Each hc In hullComponents(GetType(hcJumpDrive))
                    Dim j As hcJumpDrive = CType(hc, hcJumpDrive)
                    If j.isActive = True Then totalSpeed += j.jumpSpeed
                Next
            Else
                For Each hc In hullComponents(GetType(hcEngine))
                    Dim e As hcEngine = CType(hc, hcEngine)
                    If e.isActive = True Then totalSpeed += e.speed
                Next
            End If
            Return totalSpeed
        End Get
    End Property
    Friend ReadOnly Property travelByJump As Boolean
        Get
            Return _travelByJump
        End Get
    End Property
    Friend Sub tickTravel()
        Dim totalSpeed As Integer = 0
        For Each kvp In hullComponents
            For Each hc In kvp.Value
                hc.tickTravel()
            Next
        Next
    End Sub
    Friend Sub tickIdle()
        For Each kvp In hullComponents
            For Each hc In kvp.Value
                hc.tickIdle()
            Next
        Next
    End Sub

    Private shields As Integer
    Private shieldsMax As Integer
    Private armour As Integer
    Private armourMax As Integer
    Private ReadOnly Property dodge As Integer
        Get
            Dim total As Integer = 0
            For Each hc In hullComponents(GetType(hcEngine))
                Dim e As hcEngine = CType(hc, hcEngine)
                If e.isActive = True Then total += e.dodge
            Next
            Return total
        End Get
    End Property
    Friend Sub fullRepair()
        shields = shieldsMax
        armour = armourMax
    End Sub
    Friend Sub addDamage(ByVal damage As Integer, ByVal damageType As eDamageType)
        If shields > 0 Then
            shields -= damage
            If shields < 0 Then
                damage = shields * -1
                shields = 0
                alert.Add("Shields Down", name & "'s shields are down.", 2)
            Else
                alert.Add("Shields", name & " has " & shields & " shields remaining.", 2)
            End If
        End If
        If damage > 0 Then
            armour -= damage
            If armour <= 0 Then destroy() Else alert.Add("Armour", name & " has " & armour & " armour remaining.", 2)
        End If
    End Sub
    Private Sub destroy()
        alert.Add("Ship Destruction", _name & " was destroyed!", 0)
    End Sub

    Private hullComponents As New Dictionary(Of Type, List(Of hullComponent))
    Private hullSpaceMax As Integer
    Private ReadOnly Property hullSpaceOccupied As Integer
        Get
            Dim total As Integer = 0
            For Each kvp In hullComponents
                For Each hc In kvp.Value
                    total += hc.size
                Next
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
        hullComponents(hc.GetType).Add(hc)
        hc.ship = Me
        mustRefresh = True
    End Sub
    Friend Sub removeComponent(ByRef hc As hullComponent)
        If hullComponents.ContainsKey(hc.GetType) = False Then Exit Sub
        If hullComponents(hc.GetType).Contains(hc) = False Then Exit Sub

        hc.ship = Nothing
        hullComponents(hc.GetType).Remove(hc)
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
    Friend Sub allLoadResource()
        For Each kvp In hullComponents
            For Each hc In kvp.Value
                hc.loadResource()
            Next
        Next
    End Sub
    Private Function getCrew() As List(Of crew)
        Dim total As New List(Of crew)
        For Each hc In hullComponents(GetType(hcCrewQuarters))
            Dim cq As hcCrewQuarters = CType(hc, hcCrewQuarters)
            total.AddRange(cq.crewList)
        Next
        Return total
    End Function
    Friend Function addCrew(ByRef crew As crew) As Boolean
        For Each hc In hullComponents(GetType(hcCrewQuarters))
            Dim cq As hcCrewQuarters = CType(hc, hcCrewQuarters)
            If cq.crewEmpty > 0 Then
                cq.addCrew(crew)
                Return True
            End If
        Next
        Return False
    End Function
End Class
