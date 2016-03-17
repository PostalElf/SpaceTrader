Public Class ship
    Public Sub New()
        For Each r In constants.resourceArray
            resources.Add(r, 0)
            resourcesMaxBase.Add(r, 10)
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
        Select Case type
            Case eShipType.Corvette
                hullSpaceMaxBase = 20
                shieldsMaxBase = 5
                armourMaxBase = 10
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Frigate
                hullSpaceMaxBase = 50
                shieldsMaxBase = 10
                armourMaxBase = 10
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Crusier
                hullSpaceMaxBase = 75
                shieldsMaxBase = 20
                armourMaxBase = 10
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Destroyer
                hullSpaceMaxBase = 100
                shieldsMaxBase = 20
                armourMaxBase = 20
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Dreadnought
                hullSpaceMaxBase = 150
                shieldsMaxBase = 20
                armourMaxBase = 40
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
        End Select
    End Sub
    Private Shared shipNames As New List(Of String)

    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftlen As Integer = 11

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Credits:", ftlen) & "¥" & player.credits.ToString("N0"))
        Console.Write(indd & fakeTab("Location:", ftlen))
        If _planet Is Nothing Then Console.WriteLine(travelDescription) Else Console.WriteLine(_planet.ToString)
        If travelDestination Is Nothing = False Then Console.WriteLine(indd & fakeTab("Target:", ftlen) & travelDestination.name)
        Console.WriteLine(indd & fakeTab("Shields:", ftlen) & shields & "/" & shieldsMax)
        Console.WriteLine(indd & fakeTab("Armour:", ftlen) & armour & "/" & armourMaxBase)
        Console.WriteLine(indd & fakeTab("Speed:", ftlen) & travelSpeed(True) & " jump + " & travelSpeed(False) & " sublight")

        Console.WriteLine(indd & fakeTab("Hull:", ftlen) & hullSpaceOccupied & "/" & hullSpaceMaxBase)
        consoleReportHullComponents(indent + 2, "└ ")

        consoleReportCrew(indent + 1)
        consoleReportCargo(indent + 1)
    End Sub
    Private Sub consoleReportHullComponents(ByVal indent As Integer, Optional ByVal prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix

        Dim ftlen As Integer = 0
        For Each hc In hullComponentsList
            If hc.name.Length > ftlen Then ftlen = hc.name.Length
        Next
        ftlen += 3

        For Each hc In hullComponentsList
            Console.WriteLine(ind & fakeTab(hc.name & ":", ftlen) & hc.consoleDescription & " " & hc.consoleResourceDescription)
        Next
    End Sub
    Private Sub consoleReportCrew(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)

        Dim crewList As List(Of crew) = getCrews()
        If crewList.Count > 0 Then
            Dim ftlen As Integer = 0
            For Each crew In crewList
                If crew.name.Length > ftlen Then ftlen = crew.name.Length
            Next
            ftlen += 3

            Console.WriteLine(ind & "Crew:")
            For Each crew In crewList
                Console.Write(indd & fakeTab("└ " & crew.name, ftlen))
                If crew.crewAssignment Is Nothing = False Then Console.Write(" (" & crew.crewAssignment.hullComponent.name & ")")
                Console.WriteLine()
            Next
        End If
    End Sub
    Private Sub consoleReportCargo(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)

        Console.WriteLine(ind & "Cargo Bay:")
        For Each r In constants.resourceArray
            Console.WriteLine(indd & "└ " & fakeTab(r.ToString & ":", 13) & resources(r) & "/" & resourcesMax(r))
        Next
    End Sub
    Friend Sub consoleReportAlarms(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)

        For Each hc In hullComponentsList
            For Each alarm In hc.alarms
                Console.WriteLine(ind & hc.name & ": " & alarm)
            Next
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

    Private _planet As planet
    Friend ReadOnly Property planet As planet
        Get
            Return _planet
        End Get
    End Property
    Friend ReadOnly Property star As star
        Get
            Return planet.star
        End Get
    End Property
    Private travelDescription As String
    Private travelJumpSpeedBase As Integer
    Private travelSublightSpeedBase As Integer
    Friend ReadOnly Property travelSpeed(ByVal jump As Boolean) As Integer
        Get
            Dim totalSpeed As Integer = 0
            If jump = True Then
                totalSpeed += travelJumpSpeedBase
                For Each hc As hcJumpDrive In hullComponents(GetType(hcJumpDrive))
                    If hc.isActive = True Then totalSpeed += hc.jumpSpeed
                Next
            Else
                totalSpeed += travelSublightSpeedBase
                For Each hc As hcEngine In hullComponents(GetType(hcEngine))
                    If hc.isActive = True Then totalSpeed += hc.speed
                Next
            End If
            Return totalSpeed
        End Get
    End Property
    Private travelDestination As planet
    Private travelProgress As Integer
    Private travelDistancePlanet1 As Integer
    Private travelDistancePlanet2 As Integer
    Private travelDistanceStar As Integer
    Friend Sub setTravelDestination(ByVal destination As planet)
        If _planet Is Nothing Then Exit Sub
        If destination Is Nothing = False AndAlso destination.Equals(_planet) Then Exit Sub

        If destination Is Nothing Then
            travelDistanceStar = 0
            travelDistancePlanet1 = 0
            travelDistancePlanet2 = 0
        ElseIf star.Equals(destination.star) Then
            travelDistanceStar = 0
            travelDistancePlanet1 = 0
            travelDistancePlanet2 = _planet.getDistanceTo(destination)
        Else
            travelDistanceStar = star.getDistanceTo(destination.star)
            travelDistancePlanet1 = _planet.distanceToGate
            travelDistancePlanet2 = destination.distanceToGate
        End If

        travelDestination = destination
        travelProgress = 0
    End Sub
    Friend Sub teleportTo(ByRef destination As planet)
        _planet = destination
        travelDestination = Nothing
        travelProgress = 0
        travelDistanceStar = 0
        travelDistancePlanet1 = 0
        travelDistancePlanet2 = 0
    End Sub
    Private Sub tickTravel()
        If travelDestination Is Nothing Then Exit Sub

        _planet = Nothing
        For Each hc In hullComponentsList
            hc.tickTravel()
        Next

        If travelDistancePlanet1 > 0 Then
            travelProgress += travelSpeed(False)
            travelDescription = "Travelling to Jump Gate (" & travelProgress & "/" & travelDistancePlanet1 & ")"
            If travelProgress >= travelDistancePlanet1 Then
                travelProgress = 0
                travelDistancePlanet1 = 0
                travelDescription = "Entering Jump Gate..."
            End If
        ElseIf travelDistanceStar > 0 Then
            travelProgress += travelSpeed(True)
            travelDescription = "Warping to " & travelDestination.star.name & " (" & travelProgress & "/" & travelDistanceStar & ")"
            If travelProgress >= travelDistanceStar Then
                travelProgress = 0
                travelDistanceStar = 0
                travelDescription = "Exiting Jump Gate..."
            End If
        ElseIf travelDistancePlanet2 > 0 Then
            travelProgress += travelSpeed(False)
            travelDescription = "Travelling to " & travelDestination.name & " (" & travelProgress & "/" & travelDistancePlanet2 & ")"
            If travelProgress >= travelDistancePlanet2 Then
                teleportTo(travelDestination)
            End If
        End If
    End Sub
    Private Sub tickIdle()
        For Each hc In hullComponentsList
            hc.tickIdle()
        Next
    End Sub

    Friend Sub tick()
        If travelDestination Is Nothing Then tickIdle() Else tickTravel()
    End Sub

    Private shields As Integer
    Private shieldsMaxBase As Integer
    Private ReadOnly Property shieldsMax As Integer
        Get
            Dim total As Integer = shieldsMaxBase
            For Each hc As hcDefence In hullComponents(GetType(hcDefence))
                If hc.defType = eDefenceType.Shields Then total += hc.value
            Next
            Return total
        End Get
    End Property
    Private armour As Integer
    Private armourMaxBase As Integer
    Private ReadOnly Property armourMax As Integer
        Get
            Dim total As Integer = armourMaxBase
            For Each hc As hcDefence In hullComponents(GetType(hcDefence))
                If hc.defType = eDefenceType.Armour Then total += hc.value
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property dodge As Integer
        Get
            Dim total As Integer = 0
            For Each hc As hcEngine In hullComponents(GetType(hcEngine))
                If hc.isActive = True Then total += hc.dodge
            Next
            Return total
        End Get
    End Property
    Friend Function getDefences(ByVal defenceType As String) As Integer()
        Dim total(1) As Integer
        Select Case defenceType.ToLower
            Case "shields"
                total(0) = shields
                total(1) = shieldsMax
            Case "armour"
                total(0) = armour
                total(1) = armourMax
            Case "dodge"
                total(0) = dodge
                total(1) = dodge
            Case Else
                MsgBox("getDefences unrecognised defenceType string")
                Return Nothing
        End Select
        Return total
    End Function
    Friend Sub fullRepair()
        shields = shieldsMax
        armour = armourMaxBase
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
                Exit Sub
            End If
        End If
        If damage > 0 Then
            armour -= damage
            If armour <= 0 Then destroy() Else alert.Add("Armour", name & " has " & armour & " armour remaining.", 2)
        End If
    End Sub
    Friend Sub repair(ByVal value As Integer)
        armour = constrain(armour + value, 0, armourMax)
    End Sub
    Private Sub destroy()
        alert.Add("Ship Destruction", _name & " was destroyed!", 0)
    End Sub

    Private hullComponents As New Dictionary(Of Type, List(Of hullComponent))
    Friend ReadOnly Property hullComponentsList As List(Of hullComponent)
        Get
            Dim total As New List(Of hullComponent)
            For Each kvp In hullComponents
                For Each hc In kvp.Value
                    total.Add(hc)
                Next
            Next
            Return total
        End Get
    End Property
    Private hullSpaceMaxBase As Integer
    Private ReadOnly Property hullSpaceMax As Integer
        Get
            Dim total As Integer = hullSpaceMaxBase
            For Each hc As hcDefence In hullComponents(GetType(hcDefence))
                If hc.defType = eDefenceType.Shields Then total += hc.value
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property hullSpaceOccupied As Integer
        Get
            Dim total As Integer = 0
            For Each hc In hullComponentsList
                total += hc.size
            Next
            Return total
        End Get
    End Property
    Private ReadOnly Property hullSpaceEmpty As Integer
        Get
            Return hullSpaceMaxBase - hullSpaceOccupied
        End Get
    End Property
    Friend Sub addComponent(ByRef hc As hullComponent)
        If hc Is Nothing Then Exit Sub
        hullComponents(hc.GetType).Add(hc)
        hc.ship = Me
    End Sub
    Friend Sub removeComponent(ByRef hc As hullComponent)
        If hullComponents.ContainsKey(hc.GetType) = False Then Exit Sub
        If hullComponents(hc.GetType).Contains(hc) = False Then Exit Sub

        hc.ship = Nothing
        hullComponents(hc.GetType).Remove(hc)
    End Sub
    Friend Function getComponents(ByVal hcType As Type) As List(Of hullComponent)
        If hullComponents.ContainsKey(hcType) Then Return hullComponents(hcType)
        Return Nothing
    End Function

    Private resources As New Dictionary(Of eResource, Integer)
    Private resourcesMaxBase As New Dictionary(Of eResource, Integer)
    Private ReadOnly Property resourcesMax(ByVal resource As eResource)
        Get
            Dim total As Integer = resourcesMaxBase(resource)
            For Each hc As hcCargo In hullComponents(GetType(hcCargo))
                If hc.resource = resource Then total += hc.value
            Next
            Return total
        End Get
    End Property
    Friend Function addResourceCheck(ByVal resource As eResource, ByVal value As Integer) As Boolean
        If resources(resource) + value > resourcesMax(resource) Then Return False
        If resources(resource) + value < 0 Then Return False

        Return True
    End Function
    Friend Sub addResource(ByVal resource As eResource, ByVal value As Integer)
        If resources(resource) + value > resourcesMax(resource) Then
            Dim waste As Integer = resources(resource) + value - resourcesMax(resource)
            alert.Add("Jettison", waste & " pod(s) of " & resource.ToString & " has been jettisoned.", 5)
            resources(resource) = resourcesMax(resource)
        Else
            resources(resource) += value
            If value > 0 Then
                alert.Add("Resource", value & " pod(s) of " & resource.ToString & " has been added to the cargo hold.", 9)
            Else
                alert.Add("Resource", Math.Abs(value) & " pod(s) of " & resource.ToString & " has been removed from the cargo hold.", 9)
            End If

        End If
    End Sub
    Friend Function getResource(ByVal resource As eResource) As Integer()
        Dim total(1) As Integer
        total(0) = resources(resource)
        total(1) = resourcesMax(resource)
        Return total
    End Function
    Friend Sub allLoadResource()
        For Each hc In hullComponentsList
            hc.loadResource()
        Next
    End Sub

    Friend Function getCrews(Optional ByVal isIdleOnly As Boolean = False) As List(Of crew)
        Dim total As New List(Of crew)
        For Each hc In hullComponents(GetType(hcCrewQuarters))
            Dim cq As hcCrewQuarters = CType(hc, hcCrewQuarters)
            If isIdleOnly = False Then
                total.AddRange(cq.crewList)
            Else
                For Each p In cq.crewList
                    If p.crewAssignment Is Nothing Then total.Add(p)
                Next
            End If
        Next
        Return total
    End Function
    Friend Function addCrew(ByRef crew As crew) As Boolean
        For Each hc In hullComponents(GetType(hcCrewQuarters))
            Dim cq As hcCrewQuarters = CType(hc, hcCrewQuarters)
            If cq.addCrewCheck(crew) = True Then
                cq.addCrew(crew)
                Return True
            End If
        Next
        Return False
    End Function
    Friend Sub allAssignCrewBest()
        For Each hc In hullComponentsList
            If TypeOf hc Is ihcCrewable Then
                Dim c As ihcCrewable = CType(hc, ihcCrewable)
                With c.crewable
                    If .isManned = False Then
                        .assignCrewBest()
                    End If
                End With
            End If
        Next
    End Sub
End Class
