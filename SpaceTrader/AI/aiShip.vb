Public Class aiShip
    Inherits ship
    Friend Shared Shadows Function build(ByRef aPlayer As aiPlayer, ByVal aType As eShipType) As aiShip
        Dim aiShip As aiShip = ship.buildAiShip(aPlayer, aType)
        aiShip.role = buildRole()
        aiShip.isAggressive = coinFlip()
        aiShip.outfitShip()
        aiShip.fullRepair()
        Return aiShip
    End Function
    Private Shared Function buildRole() As eAiShipRole
        'Return rng.Next(1, System.Enum.GetValues(GetType(eAiShipRole)).Length - 1)
        Return eAiShipRole.Artillery
    End Function

    Private Sub outfitShip()
        Select Case type
            Case eShipType.Corvette
                Select Case role
                    Case eAiShipRole.Striker
                        outfitShipResources(3, eResource.Ammunition)
                        outfitShipResources(1, eResource.Chemicals)
                        addComponent(hullComponent.build("Laser Array"))
                        addComponent(hullComponent.build("Laser Array"))
                        addComponent(hullComponent.build("Mass Driver"))
                        addComponent(hullComponent.build("'Galapagos' Viral Broadcaster"))

                    Case eAiShipRole.Tank
                        outfitShipResources(3, eResource.Ammunition)
                        outfitShipResources(1, eResource.Chemicals)
                        addComponent(hullComponent.build("Weavespun Bubble"))
                        addComponent(hullComponent.build("Steelfoam Plating"))
                        addComponent(hullComponent.build("Sentinel Laser Grid"))
                        addComponent(hullComponent.build("Mass Driver"))

                    Case eAiShipRole.Artillery
                        outfitShipResources(3, eResource.Missiles)
                        addComponent(hullComponent.build("Weavespun Bubble"))
                        addComponent(hullComponent.build("Sentinel Laser Grid"))
                        addComponent(hullComponent.build("Hellfire Missiles"))
                        addComponent(hullComponent.build("Hellfire Missiles"))

                    Case eAiShipRole.Beehive
                        outfitShipResources(3, eResource.Machines)

                    Case eAiShipRole.Hacker


                    Case Else
                        MsgBox("aiShip.outfitShip: role out of bounds.")
                        Exit Sub
                End Select

            Case Else
                MsgBox("aiShip.outfitShip: type out of bounds.")
                Exit Sub
        End Select
    End Sub
    Private Sub outfitShipResources(ByVal value As Integer, Optional ByVal r As eResource = Nothing)
        If r = Nothing Then
            For Each r In constants.resourceArray
                resourcesMaxBase(r) = value
                addResource(r, value)
            Next
        Else
            resourcesMaxBase(r) = value
            addResource(r, value)
        End If
    End Sub
    Friend Overrides Sub addComponent(ByRef hc As hullComponent)
        MyBase.addComponent(hc)
        hc.autoloadResource = True
    End Sub

    Private role As eAiShipRole
    Private isAggressive As Boolean
    Friend Overrides Sub tickCombat()
        MyBase.tickCombat()
        If isAggressive = True Then tickCombatAggressive() Else tickCombatDefensive()

        enemyAttacksMadeLastTurn.Clear()
        player.alertsClear()
    End Sub
    Private Sub tickCombatAggressive()
        Dim highestEnergyCost As Integer = 0
        Dim mcat As eDamageType = getMostCommonAttackType()

        'use special components
        useSpecialComponents()

        'spend energy on attacks
        If enemyInterceptors.Count > 0 Then attackInterceptors()
        While combatEnergy >= highestEnergyCost
            Dim target As ship = battlefield.getEnemyShipRandom(player)
            If target Is Nothing Then Exit Sub
            Dim dodge As Integer = target.getDefences(eDefenceType.Dodge)(0)
            Dim weapon As hcWeapon = Nothing

            'try to get best weapon with accuracy greater than dodge
            'if not possible, try to get best weapon
            'if not possible
            If dodge > 0 Then weapon = getWeaponBest(getWeapons(dodge), target)
            If weapon Is Nothing Then weapon = getWeaponBest(getWeapons(0), target)
            If weapon Is Nothing Then Exit While Else weapon.UseCombat(target)
            highestEnergyCost = getHighestEnergyCost()
        End While

        'spend leftover energy on defence
        Select Case mcat
            Case eDamageType.Digital : addBoost(eDefenceType.Firewall, combatEnergy)
            Case eDamageType.Missile : addBoost(eDefenceType.PointDefence, combatEnergy)
            Case Else : addBoost(eDefenceType.Dodge, combatEnergy)
        End Select
    End Sub
    Private Sub tickCombatDefensive()
        Dim highestEnergyCost As Integer = 0
        Dim mcat As eDamageType = getMostCommonAttackType()

        'use special components
        useSpecialComponents()

        'attack interceptors
        If enemyInterceptors.Count > 0 Then attackInterceptors()

        'defend with half energy or highest accuracy, whichever is lower
        Dim budget As Integer = Math.Min(combatEnergy / 2, getMostCommonAttackAccuracy())
        Select Case mcat
            Case eDamageType.Digital : addBoost(eDefenceType.Firewall, budget)
            Case eDamageType.Missile : addBoost(eDefenceType.PointDefence, budget)
            Case Else : addBoost(eDefenceType.Dodge, budget)
        End Select

        'spend leftover energy on attacks
        While combatEnergy >= highestEnergyCost
            Dim target As ship = battlefield.getEnemyShipRandom(player)
            If target Is Nothing Then Exit Sub
            Dim dodge As Integer = target.getDefences(eDefenceType.Dodge)(0)
            Dim weapon As hcWeapon = Nothing

            'try to get best weapon with accuracy greater than dodge
            'if not possible, try to get best weapon
            'if not possible
            If dodge > 0 Then weapon = getWeaponBest(getWeapons(dodge), target)
            If weapon Is Nothing Then weapon = getWeaponBest(getWeapons(0), target)
            If weapon Is Nothing Then Exit While Else weapon.UseCombat(target)
            highestEnergyCost = getHighestEnergyCost()
        End While
    End Sub
    Private Sub useSpecialComponents()
        For Each hcr As hcRepairer In hullComponents(GetType(hcRepairer))
            hcr.UseCombat(Me)
        Next
    End Sub
    Private Sub attackInterceptors()
        Dim pd As hcDefence = getPointDefenceCheapest()
        If pd Is Nothing Then Exit Sub
        Dim chance As Integer
        If isAggressive = False Then chance = 75 Else chance = 50

        For n = enemyInterceptors.Count - 1 To 0 Step -1
            If percentRoll(chance) = True Then
                Dim interceptor As interceptor = enemyInterceptors(n)
                pd.UseCombat(interceptor)
            End If
        Next
    End Sub
    Private Function getHighestEnergyCost() As Integer
        Dim total As Integer = 0
        For Each hc As hcWeapon In hullComponents(GetType(hcWeapon))
            If hc.UseCombatCheck(Nothing) = True AndAlso hc.energyCost > total Then total = hc.energyCost
        Next
        For Each hc As hcDefence In getPointDefences()
            If hc.UseCombatCheck(Nothing) = True AndAlso hc.energyCost > total Then total = hc.energyCost
        Next
        Return total
    End Function
    Private Function getPointDefences() As List(Of hcDefence)
        Dim total As New List(Of hcDefence)
        For Each hcd As hcDefence In hullComponents(GetType(hcDefence))
            If hcd.defType = eDefenceType.PointDefence Then total.Add(hcd)
        Next
        Return total
    End Function
    Private Function getPointDefenceCheapest() As hcDefence
        Dim min As hcDefence = Nothing
        Dim minCost As Integer = Int32.MaxValue
        For Each hcd As hcDefence In getPointDefences()
            If hcd.energyCost < minCost Then
                min = hcd
                minCost = hcd.energyCost
            End If
        Next
        Return min
    End Function
    Private Function getWeapons(ByVal minAccuracy As Integer) As List(Of hcWeapon)
        Dim total As New List(Of hcWeapon)
        For Each hcw As hcWeapon In hullComponents(GetType(hcWeapon))
            If hcw.damage.accuracy >= minAccuracy Then total.Add(hcw)
        Next
        Return total
    End Function
    Private Function getWeaponBest(ByVal weaponList As List(Of hcWeapon), ByVal target As ship) As hcWeapon
        If weaponList.Count = 0 Then Return Nothing

        Dim best As hcWeapon = Nothing
        Dim bestValue As Double = 0
        For Each hcw In weaponList
            Dim aiValue As Double = getWeaponValue(hcw, target)
            If aiValue > bestValue Then
                best = hcw
                bestValue = aiValue
            End If
        Next
        Return best
    End Function
    Private Function getWeaponValue(ByRef hcw As hcWeapon, ByVal target As ship) As Double
        If hcw.useResourceCheck() = False Then hcw.loadResource()
        If hcw.useResourceCheck() = False Then Return -1
        If hcw.UseCombatCheck(target) = False Then Return -1

        Dim total As Double
        If hcw.isCarrier = True Then
            total += 0.5
            If role = eAiShipRole.Beehive Then total += 0.5
        End If
        With hcw.damage
            total += (.damageFull + .damageGlancing) / 2
            If .type = eDamageType.Energy AndAlso target.getDefences(eDefenceType.Shields)(0) > 0 Then total *= 1.5
            If .digitalPayload <> Nothing Then
                total += 0.5
                If role = eAiShipRole.Hacker Then total += 0.5
            End If
        End With
        Return total / hcw.energyCost
    End Function

    Private enemyAttacksMadeLastTurn As New List(Of damage)
    Private Function getMostCommonAttackType() As eDamageType
        If enemyAttacksMadeLastTurn.Count = 0 Then Return eDamageType.Ballistic

        Dim damageTypes As New Dictionary(Of eDamageType, Integer)
        For Each ea In enemyAttacksMadeLastTurn
            If damageTypes.ContainsKey(ea.type) = False Then damageTypes.Add(ea.type, 0)
            damageTypes(ea.type) += 1
        Next

        Return damageTypes.Max.Key
    End Function
    Private Function getMostCommonAttackAccuracy() As Integer
        If enemyAttacksMadeLastTurn.Count = 0 Then Return 0

        Dim total As Integer = 0
        For Each d In enemyAttacksMadeLastTurn
            If d.accuracy > total Then total = d.accuracy
        Next
        Return total
    End Function
    Friend Overrides Sub addDamage(ByRef attacker As iCombatant, ByVal damage As damage)
        enemyAttacksMadeLastTurn.Add(damage)
        MyBase.addDamage(attacker, damage)
    End Sub
End Class
