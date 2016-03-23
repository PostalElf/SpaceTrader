Public Class aiShip
    Inherits ship
    Friend Shared Shadows Function build(ByRef aPlayer As aiPlayer, ByVal aType As eShipType) As aiShip
        Dim aiShip As aiShip = ship.buildAiShip(aPlayer, aType)
        aiShip.role = buildRole()
        'aiShip.isAggressive = coinFlip()
        aiShip.isAggressive = True
        aiShip.outfitShip()
        Return aiShip
    End Function
    Private Shared Function buildRole() As eAiShipRole
        'Return rng.Next(1, System.Enum.GetValues(GetType(eAiShipRole)).Length - 1)
        Return eAiShipRole.Striker
    End Function
    Private Sub outfitShip()
        Select Case type
            Case eShipType.Corvette
                Select Case role
                    Case eAiShipRole.Striker
                        addComponent(New hcWeapon("Mass Driver", 5, 5, eDamageType.Ballistic, 20, 3, 1, Nothing))
                        addComponent(New hcWeapon("Spiral-X Missiles", 5, 5, eDamageType.Missile, 10, 3, 3, Nothing))
                        addComponent(New hcDefence("Quantum Shield Battery", 5, eDefenceType.Shields, 10))
                        Dim pd As New hcDefence("Point-Defence Turret", 5, eDefenceType.PointDefence, 5)
                        pd.pdEnergyCost = 5
                        addComponent(pd)
                    Case eAiShipRole.Tank
                    Case eAiShipRole.Artillery
                    Case eAiShipRole.Beehive
                    Case Else
                        MsgBox("aiShip.outfitShip: role out of bounds.")
                        Exit Sub
                End Select

            Case Else
                MsgBox("aiShip.outfitShip: type out of bounds.")
                Exit Sub
        End Select
    End Sub

    Private role As eAiShipRole
    Private isAggressive As Boolean
    Friend Overrides Sub tickCombat()
        If isAggressive = True Then tickCombatAggressive() Else tickCombatDefensive()

        MyBase.tickCombat()
        enemyAttacksMadeLastTurn.Clear()
        player.alertsClear()
    End Sub
    Private Sub tickCombatAggressive()
        Dim highestEnergyCost As Integer = getHighestEnergyCost()
        Dim mcat As eDamageType = getMostCommonAttackType()

        'spend energy on attacks
        If enemyInterceptors.Count > 0 Then attackInterceptors()
        While combatEnergy >= highestEnergyCost
            Dim target As ship = battlefield.getEnemyShipRandom(player)
            Dim dodge As Integer = target.getDefences(eDefenceType.Dodge)(0)
            Dim weapon As hcWeapon = Nothing
            If dodge > 0 Then weapon = getWeaponBest(getWeapons(dodge), target)
            If weapon Is Nothing Then weapon = getWeaponBest(getWeapons(0), target)
            weapon.attack(target)
        End While

        'spend leftover energy on defence
        Select Case mcat
            Case eDamageType.Digital : addBoost(eDefenceType.Firewall, combatEnergy)
            Case eDamageType.Missile : addBoost(eDefenceType.PointDefence, combatEnergy)
            Case Else : addBoost(eDefenceType.Dodge, combatEnergy)
        End Select
    End Sub
    Private Sub tickCombatDefensive()

    End Sub
    Private Sub attackInterceptors()
        Dim pd As hcDefence = getPointDefenceCheapest()
        Dim chance As Integer
        If isAggressive = False Then chance = 75 Else chance = 50

        For n = enemyInterceptors.Count - 1 To 0 Step -1
            If percentRoll(chance) = True Then
                Dim interceptor As interceptor = enemyInterceptors(n)
                pd.pdAttack(interceptor)
            End If
        Next
    End Sub
    Private Function getHighestEnergyCost() As Integer
        Dim total As Integer = 0
        For Each hc As hcWeapon In hullComponents(GetType(hcWeapon))
            If hc.energyCost > total Then total = hc.energyCost
        Next
        For Each hc As hcDefence In getPointDefences()
            If hc.pdEnergyCost > total Then total = hc.pdEnergyCost
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
            If hcd.pdEnergyCost < minCost Then
                min = hcd
                minCost = hcd.pdEnergyCost
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
    Private Function getWeaponValue(ByVal hcw As hcWeapon, ByVal target As ship) As Double
        Dim total As Double
        With hcw.damage
            total += (.damageFull + .damageGlancing) / 2
            If .type = eDamageType.Energy AndAlso target.getDefences(eDefenceType.Shields)(0) > 0 Then total *= 1.5
            If .digitalPayload <> Nothing Then total += 1.5
        End With
        Return total
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
    Friend Overrides Sub addDamage(ByRef attacker As iCombatant, ByVal damage As damage)
        enemyAttacksMadeLastTurn.Add(damage)
        MyBase.addDamage(attacker, damage)
    End Sub
End Class
