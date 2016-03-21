﻿Public Class hcWeapon
    Inherits hullComponent
    Implements ihcCrewable
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aEnergyCost As Integer, _
                   ByVal aDamageType As eDamageType, ByVal aAccuracy As Integer, ByVal aDamageFull As Integer, ByVal aDamageGlancing As Integer, ByVal digitalPayload As eDigitalAttack, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)

        energyCost = aEnergyCost
        If aDamageType = eDamageType.Interceptors Then
            isCarrier = True
            damage = New damage(eDamageType.Ballistic, aAccuracy, aDamageFull, aDamageGlancing, digitalPayload)
        Else
            isCarrier = False
            damage = New damage(aDamageType, aAccuracy, aDamageFull, aDamageGlancing, digitalPayload)
        End If
    End Sub
    Friend Overrides Function consoleDescription() As String
        With damage
            Return .damageGlancing & "-" & .damageFull & " " & .type.ToString & " damage @ " & .accuracy & " accuracy"
        End With
    End Function
    Friend Overrides ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)(MyBase.alarms)
            If crewable.isManned = False Then total.Add("Requires crew member(s).")
            Return total
        End Get
    End Property

    Private energyCost As Integer
    Private isCarrier As Boolean
    Friend interceptorName As String
    Private damage As damage
    Friend Sub attack(ByRef target As ship)
        If crewable.isManned = False Then Exit Sub
        If useResource() = False Then Exit Sub

        If isCarrier = True Then
            target.addInterceptor(ship, New interceptor(interceptorName, ship, target, damage))
        Else
            target.addDamage(ship, damage)
        End If
    End Sub

    Friend Property crewable As New shcCrewable(Me) Implements ihcCrewable.crewable
    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Weapon"
        End Get
    End Property
End Class
