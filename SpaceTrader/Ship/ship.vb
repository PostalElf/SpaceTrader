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
            Case eShipType.Frigate
                hullSpaceMaxBase = 50
                shieldsMaxBase = 10
                armourMaxBase = 10
            Case eShipType.Crusier
                hullSpaceMaxBase = 75
                shieldsMaxBase = 20
                armourMaxBase = 10
            Case eShipType.Destroyer
                hullSpaceMaxBase = 100
                shieldsMaxBase = 20
                armourMaxBase = 20
            Case eShipType.Dreadnought
                hullSpaceMaxBase = 150
                shieldsMaxBase = 20
                armourMaxBase = 40
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
        Console.WriteLine(indd & fakeTab("Shields:", ftlen) & shields & "/" & shieldsMax)
        Console.WriteLine(indd & fakeTab("Armour:", ftlen) & armour & "/" & armourMaxBase)
        Console.Write(indd & fakeTab("Speed:", ftlen) & travelSpeed & " ")
        If travelByJump = True Then Console.WriteLine("jump") Else Console.WriteLine("sublight")

        Console.WriteLine(indd & fakeTab("Hull:", ftlen) & hullSpaceOccupied & "/" & hullSpaceMaxBase)
        consoleReportHullComponents(indent + 2, "└ ")

        Dim crewList As List(Of crew) = getCrews()
        If crewList.Count > 0 Then
            Console.WriteLine(indd & "Crew:")
            For Each crew In crewList
                Console.Write(inddd & "└ " & crew.name)
                If crew.crewAssignment Is Nothing = False Then Console.Write(" (" & crew.crewAssignment.hullComponent.name & ")")
                Console.WriteLine()
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
    Friend Sub consoleReportAlarms(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)

        For Each kvp In hullComponents
            Dim hcList As List(Of hullComponent) = kvp.Value
            For Each hc In hcList
                For Each alarm In hc.alarms
                    Console.WriteLine(ind & hc.name & ": " & alarm)
                Next
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

    Private _travelByJump As Boolean = True
    Friend ReadOnly Property travelByJump As Boolean
        Get
            Return _travelByJump
        End Get
    End Property
    Friend ReadOnly Property travelSpeed As Integer
        Get
            Dim totalSpeed As Integer = 0
            If travelByJump = True Then
                For Each hc As hcJumpDrive In hullComponents(GetType(hcJumpDrive))
                    If hc.isActive = True Then totalSpeed += hc.jumpSpeed
                Next
            Else
                For Each hc As hcEngine In hullComponents(GetType(hcEngine))
                    If hc.isActive = True Then totalSpeed += hc.speed
                Next
            End If
            Return totalSpeed
        End Get
    End Property
    Private planet As planet
    Private travelDestination As planet
    Private travelProgress As Integer
    Private travelDistance As Integer
    Friend Sub setTravelDestination(ByVal destination As planet)
        If planet Is Nothing Then Exit Sub

        travelDestination = destination
        travelProgress = 0
        travelDistance = planet.getDistanceTo(destination)
    End Sub
    Friend Sub tickTravel()
        Dim totalSpeed As Integer = 0
        If travelByJump = True Then
            For Each hc As hcJumpDrive In hullComponents(GetType(hcJumpDrive))
                hc.tickTravel()
                totalSpeed += hc.jumpSpeed
            Next
        Else
            For Each hc As hcEngine In hullComponents(GetType(hcEngine))
                hc.tickTravel()
                totalSpeed += hc.speed
            Next
        End If


    End Sub
    Friend Sub tickIdle()
        For Each kvp In hullComponents
            For Each hc In kvp.Value
                hc.tickIdle()
            Next
        Next
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
            Return hullSpaceMaxBase - hullSpaceOccupied
        End Get
    End Property
    Friend Sub addComponent(ByRef hc As hullComponent)
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
    Friend Sub allLoadResource()
        For Each kvp In hullComponents
            For Each hc In kvp.Value
                hc.loadResource()
            Next
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
End Class
