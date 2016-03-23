Public Class aiShip
    Inherits ship

    Private role As eAiShipRole
    Private isAggressive As Boolean
    Friend Overrides Sub tickCombat()
        If isAggressive = True Then tickCombatAggressive() Else tickCombatDefensive()

        MyBase.tickCombat()
        enemyAttacksMadeLastTurn.Clear()
    End Sub
    Private Sub tickCombatAggressive()
        Dim highestEnergyCost As Integer = getHighestEnergyCost()
        Dim mcat As eDamageType = getMostCommonAttackType()

        'spend energy on attacks
        While combatEnergy >= highestEnergyCost
            If enemyInterceptors.Count > 0 Then attackInterceptors()
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
