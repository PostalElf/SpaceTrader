Public Class ship
    Implements iCombatant
    Public Sub New()
        For Each r In constants.resourceArray
            resources.Add(r, 0)
            resourcesMaxBase.Add(r, 10)
        Next
        For Each t As Type In constants.hcTypeList
            hullComponents.Add(t, New List(Of hullComponent))
        Next
        For Each d In constants.defenceTypeArray
            defenceRoundBoosts.Add(d, 0)
            defenceCombatDebuffs.Add(d, 0)
        Next
    End Sub
    Private Sub initialise(ByRef aPlayer As player, ByVal aType As eShipType)
        _name = getRandomAndRemove(shipNames, "data/shipnames.txt")
        _player = aPlayer
        type = aType
        buildDefaultShip()
        fullRepair()
        aPlayer.ships.Add(Me)
    End Sub
    Friend Shared Function build(ByRef aPlayer As player, ByVal aType As eShipType) As ship
        Dim ship As New ship
        ship.initialise(aPlayer, aType)
        Return ship
    End Function
    Friend Shared Function buildAiShip(ByRef aPlayer As player, ByVal aType As eShipType) As aiShip
        Dim ship As New aiShip
        ship.initialise(aPlayer, aType)
        Return ship
    End Function
    Private Sub buildDefaultShip()
        Select Case type
            Case eShipType.Corvette
                hullSpaceMaxBase = 20
                addComponent(New hcDefence("Hull Plating", 0, eDefenceType.Armour, 10))
                addComponent(New hcDefence("Voidshields", 0, eDefenceType.Shields, 10))
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Frigate
                hullSpaceMaxBase = 50
                addComponent(New hcDefence("Hull Plating", 0, eDefenceType.Armour, 20))
                addComponent(New hcDefence("Voidshields", 0, eDefenceType.Shields, 10))
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Crusier
                hullSpaceMaxBase = 75
                addComponent(New hcDefence("Hull Plating", 0, eDefenceType.Armour, 30))
                addComponent(New hcDefence("Voidshields", 0, eDefenceType.Shields, 10))
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Destroyer
                hullSpaceMaxBase = 100
                addComponent(New hcDefence("Hull Plating", 0, eDefenceType.Armour, 50))
                addComponent(New hcDefence("Voidshields", 0, eDefenceType.Shields, 10))
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
            Case eShipType.Dreadnought
                hullSpaceMaxBase = 150
                addComponent(New hcDefence("Hull Plating", 0, eDefenceType.Armour, 50))
                addComponent(New hcDefence("Voidshields", 0, eDefenceType.Shields, 10))
                travelSublightSpeedBase = 5
                travelJumpSpeedBase = 1
        End Select
    End Sub
    Private Shared shipNames As New List(Of String)

    Public Overrides Function ToString() As String
        Return name
    End Function
    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftlen As Integer = 16

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Credits:", ftlen) & "¥" & _player.credits.ToString("N0"))
        Console.Write(indd & fakeTab("Location:", ftlen))
        If _planet Is Nothing Then Console.WriteLine(travelDescription) Else Console.WriteLine(_planet.ToString)
        If travelDestination Is Nothing = False Then Console.WriteLine(indd & fakeTab("Target:", ftlen) & travelDestination.name)
        Console.WriteLine(indd & fakeTab("Speed:", ftlen) & travelSpeed(True) & " jump + " & travelSpeed(False) & " sublight")
        Console.WriteLine(indd & fakeTab("Shields:", ftlen) & defences(eDefenceType.Shields) & "/" & defencesMax(eDefenceType.Shields))
        Console.WriteLine(indd & fakeTab("Armour:", ftlen) & defences(eDefenceType.Armour) & "/" & defencesMax(eDefenceType.Armour))
        Console.WriteLine(indd & fakeTab("Hull:", ftlen) & hullSpaceOccupied & "/" & hullSpaceMaxBase)
        consoleReportHullComponents(indent + 2, "└ ")

        consoleReportCrew(indent + 1)
        consoleReportCargo(indent + 1)
    End Sub
    Friend Sub consoleReportCombat(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Const ftlen As Integer = 16

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Energy:", ftlen) & combatEnergy & "/" & combatEnergyMax)
        For Each d As eDefenceType In constants.defenceTypeArray
            Dim def As String = d.ToString
            If d = eDefenceType.PointDefence Then def = "Point Defence"
            If d = eDefenceType.Shields OrElse d = eDefenceType.Armour Then
                Console.WriteLine(indd & fakeTab(def & ":", ftlen) & defences(d) & "/" & defencesMax(d))
            Else
                Console.WriteLine(indd & fakeTab(def & ":", ftlen) & defences(d))
            End If
        Next
        Console.WriteLine(indd & fakeTab("Hull:", ftlen) & hullSpaceOccupied & "/" & hullSpaceMaxBase)
        consoleReportHullComponents(indent + 2, "└ ")
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
    Friend ReadOnly Property name As String Implements iCombatant.name
        Get
            Return getTypePrefix() & " " & _name
        End Get
    End Property
    Private _player As player
    Friend ReadOnly Property player As player
        Get
            Return _player
        End Get
    End Property
    Friend Sub addAlert(ByVal title As String, ByVal text As String, ByVal priority As Integer) Implements iCombatant.addAlert
        player.addAlert(title, text, priority)
    End Sub
    Protected type As eShipType
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

    Friend Sub tick()
        If travelDestination Is Nothing Then tickIdle() Else tickTravel()
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

    Protected battlefield As battlefield
    Friend Sub enterCombat(ByRef aBattlefield As battlefield)
        combatEnergy = combatEnergyMax
        For Each d In constants.defenceTypeArray
            defenceRoundBoosts(d) = 0
            defenceCombatDebuffs(d) = 0
        Next

        battlefield = aBattlefield
    End Sub
    Friend Overridable Sub tickCombat()
        combatEnergy = combatEnergyMax
        For Each d In constants.defenceTypeArray
            defenceRoundBoosts(d) = 0
        Next

        For Each interceptor In enemyInterceptors
            interceptor.tickCombat()
        Next
    End Sub
    Friend Sub leaveCombat()
        combatEnergy = 0
        For Each d In constants.defenceTypeArray
            defenceRoundBoosts(d) = 0
            defenceCombatDebuffs(d) = 0
        Next

        battlefield = Nothing
    End Sub
    Private Const combatEnergyMax As Integer = 20
    Protected combatEnergy As Integer
    Friend Sub addEnergy(ByVal value As Integer)
        combatEnergy += value
        combatEnergy = constrain(combatEnergy, 0, combatEnergyMax)
    End Sub
    Friend Function addEnergyCheck(ByVal value As Integer) As Boolean
        If value < 0 Then
            If combatEnergy - value < 0 Then Return False
        End If
        Return True
    End Function
    Friend enemyInterceptors As New List(Of interceptor)
    Friend Sub addInterceptor(ByRef attacker As iCombatant, ByVal interceptor As interceptor)
        enemyInterceptors.Add(interceptor)
    End Sub
    Friend Sub removeInterceptor(ByRef interceptor As interceptor)
        enemyInterceptors.Remove(interceptor)
    End Sub

    Private shields As Integer
    Private armour As Integer
    Private Property defences(ByVal defenceType As eDefenceType) As Integer
        Get
            Dim total As Integer
            Select Case defenceType
                Case eDefenceType.Dodge
                    For Each hc As hcEngine In hullComponents(GetType(hcEngine))
                        total += hc.dodge
                    Next
                Case eDefenceType.Shields : total = shields
                Case eDefenceType.Armour : total = armour
                Case Else
                    For Each hc As hcDefence In hullComponents(GetType(hcDefence))
                        If hc.defType = defenceType Then total += hc.value
                    Next
            End Select
            total += defenceRoundBoosts(defenceType) - defenceCombatDebuffs(defenceType)
            Return total
        End Get
        Set(ByVal value As Integer)
            Select Case defenceType
                Case eDefenceType.Shields : shields = value
                Case eDefenceType.Armour : armour = value
            End Select
        End Set
    End Property
    Private defenceRoundBoosts As New Dictionary(Of eDefenceType, Integer)
    Private defenceCombatDebuffs As New Dictionary(Of eDefenceType, Integer)
    Private Function defencesMax(ByVal defenceType As eDefenceType) As Integer
        Dim total As Integer = 0
        For Each hc As hcDefence In hullComponents(GetType(hcDefence))
            If hc.defType = defenceType Then total += hc.value
        Next
        Return total
    End Function
    Friend Function getDefences(ByVal defenceType As eDefenceType) As Integer()
        Dim total(1) As Integer
        total(0) = defences(defenceType)
        total(1) = defencesMax(defenceType)
        Return total
    End Function
    Friend Sub addBoost(ByVal defenceType As eDefenceType, ByVal value As Integer)
        combatEnergy -= value
        Select Case defenceType
            Case eDefenceType.Firewall : defenceRoundBoosts(eDefenceType.Firewall) += value
            Case eDefenceType.Dodge : defenceRoundBoosts(eDefenceType.Dodge) += value
        End Select
    End Sub
    Friend Function addBoostCheck(ByVal defenceType As eDefenceType, ByVal value As Integer) As Boolean
        If combatEnergy < value Then Return False
        Return True
    End Function
    Friend Sub fullRepair()
        For Each d In constants.defenceTypeArray
            defences(d) = defencesMax(d)
        Next
    End Sub
    Friend Sub repair(ByVal defenceType As eDefenceType, ByVal value As Integer)
        defences(defenceType) = constrain(defences(defenceType) + value, 0, defencesMax(defenceType))
    End Sub
    Friend Overridable Sub addDamage(ByRef attacker As iCombatant, ByVal damage As damage)
        With damage
            If .type = eDamageType.Digital Then
                'digital attack
                player.addAlert("Attack", attacker.name & " successfully hacks into " & name & "'s systems.", 2)
                attacker.addAlert("Attack", attacker.name & " successfully hacks into " & name & "'s systems.", 2)
                If .accuracy >= defences(eDefenceType.Firewall) Then
                    Select Case .digitalPayload
                        Case eDigitalAttack.Trojan
                            defenceCombatDebuffs(eDefenceType.Firewall) += .damageFull
                            player.addAlert("Trojan", name & " has been infected with a Trojan, reducing its Firewall to " & defences(eDefenceType.Firewall) & ".", 2)
                            attacker.addAlert("Trojan", name & " has been infected with a Trojan, reducing its Firewall to " & defences(eDefenceType.Firewall) & ".", 2)
                        Case eDigitalAttack.SynapticVirus
                            defenceCombatDebuffs(eDefenceType.Dodge) += .damageFull
                            player.addAlert("Synaptic Virus", name & "'s engines have been infected with a Virus, reducing its Dodge to " & defences(eDefenceType.Dodge) & ".", 2)
                            attacker.addAlert("Synaptic Virus", name & "'s engines have been infected with a Virus, reducing its Dodge to " & defences(eDefenceType.Dodge) & ".", 2)
                        Case eDigitalAttack.NetworkWorm
                            defenceCombatDebuffs(eDefenceType.PointDefence) += .damageFull
                            player.addAlert("Network Worm", name & "'s network has been infected with a Worm, reducing its Point Defence to " & defences(eDefenceType.PointDefence) & ".", 2)
                            attacker.addAlert("Network Worm", name & "'s network has been infected with a Worm, reducing its Point Defence to " & defences(eDefenceType.PointDefence) & ".", 2)
                        Case Else
                            MsgBox("addDamage: unexpected digitalPayload")
                            Exit Sub
                    End Select
                End If
            Else
                'conventional attack
                Dim dmgValue As Integer
                If .type = eDamageType.Missile Then .accuracy -= defences(eDefenceType.PointDefence)
                If .accuracy >= defences(eDefenceType.Dodge) Then dmgValue = .damageFull Else dmgValue = .damageGlancing

                If defences(eDefenceType.Shields) > 0 Then
                    If .type = eDamageType.Energy Then dmgValue *= 1.5
                    defences(eDefenceType.Shields) -= dmgValue
                    player.addAlert("Attack", attacker.name & " hits " & name & "'s shields for " & dmgValue & " " & .type.ToString & " damage.", 2)
                    attacker.addAlert("Attack", attacker.name & " hits " & name & "'s shields for " & dmgValue & " " & .type.ToString & " damage.", 2)
                    If defences(eDefenceType.Shields) < 0 Then
                        If .type = eDamageType.Energy Then dmgValue = 0 Else dmgValue = defences(eDefenceType.Shields) * -1
                        defences(eDefenceType.Shields) = 0
                        player.addAlert("Shields Down", name & "'s shields are down.", 2)
                        attacker.addAlert("Shields Down", name & "'s shields are down.", 2)
                    Else
                        dmgValue = 0
                    End If
                End If

                If dmgValue <= 0 Then Exit Sub

                defences(eDefenceType.Armour) -= dmgValue
                player.addAlert("Attack", attacker.name & " hits " & name & "'s armour for " & dmgValue & " " & .type.ToString & " damage.", 2)
                attacker.addAlert("Attack", attacker.name & " hits " & name & "'s armour for " & dmgValue & " " & .type.ToString & " damage.", 2)
                If defences(eDefenceType.Armour) <= 0 Then
                    attacker.addAlert("Ship Destruction", name & " was destroyed!", 2)
                    destroy()
                End If
            End If
        End With
    End Sub
    Private Sub destroy() Implements iCombatant.destroy
        player.addAlert("Ship Destruction", _name & " was destroyed!", 0)

        If battlefield Is Nothing = False Then
            battlefield.ships(_player).Remove(Me)
            battlefield = Nothing
        End If

        _planet = Nothing
        travelDestination = Nothing
        defenceRoundBoosts = Nothing
        defenceCombatDebuffs = Nothing
        hullComponents = Nothing
        resources = Nothing
        resourcesMaxBase = Nothing
        craftComponents = Nothing
    End Sub

    Protected hullComponents As New Dictionary(Of Type, List(Of hullComponent))
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
    Friend Function addComponentCheck(ByRef hc As hullComponent) As Boolean
        If hc Is Nothing Then Return False
        If hullSpaceOccupied + hc.size > hullSpaceEmpty Then Return False

        Return True
    End Function
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
            player.addAlert("Jettison", waste & " pod(s) of " & resource.ToString & " has been jettisoned.", 5)
            resources(resource) = resourcesMax(resource)
        Else
            resources(resource) += value
            If value > 0 Then
                player.addAlert("Resource", value & " pod(s) of " & resource.ToString & " has been added to the cargo hold.", 9)
            Else
                player.addAlert("Resource", Math.Abs(value) & " pod(s) of " & resource.ToString & " has been removed from the cargo hold.", 9)
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

    Friend craftComponents As New Dictionary(Of String, Integer)
    Friend Sub addCraftComponent(ByVal name As String, ByVal value As Integer)
        If craftComponents.ContainsKey(name) = False Then craftComponents.Add(name, 0)
        craftComponents(name) += value
        If craftComponents(name) <= 0 Then craftComponents.Remove(name)
    End Sub
    Friend Function addCraftComponentCheck(ByVal name As String, ByVal value As Integer)
        If value < 0 Then
            If craftComponents.ContainsKey(name) = False Then Return False
            If craftComponents(name) + value < 0 Then Return False
        End If
        Return True
    End Function

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
