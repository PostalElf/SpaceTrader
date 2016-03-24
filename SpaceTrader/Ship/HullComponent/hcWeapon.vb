Public Class hcWeapon
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aEnergyCost As Integer, ByVal aInterceptorName As String, _
                   ByVal aDamageType As eDamageType, ByVal aAccuracy As Integer, ByVal aDamageFull As Integer, ByVal aDamageGlancing As Integer, ByVal digitalPayload As eDigitalAttack, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)

        _energyCost = aEnergyCost
        If aInterceptorName <> "" Then
            _isCarrier = True
            interceptorName = aInterceptorName
        Else : _isCarrier = False
        End If
        _damage = New damage(aDamageType, aAccuracy, aDamageFull, aDamageGlancing, digitalPayload)
    End Sub
    Friend Overrides Function consoleDescription() As String
        Dim total As String = ""
        With _damage
            If .damageGlancing = .damageFull Then total &= .damageGlancing Else total &= .damageGlancing & "-" & .damageFull
            total &= " " & .type.ToString & " damage @ " & .accuracy & " accuracy"
        End With
        Return total
    End Function
    Friend Overrides ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)(MyBase.alarms)
            If crewable.isManned = False Then total.Add("Requires crew member(s).")
            Return total
        End Get
    End Property

    Private _isCarrier As Boolean
    Friend ReadOnly Property isCarrier As Boolean
        Get
            Return _isCarrier
        End Get
    End Property
    Friend interceptorName As String
    Private _damage As damage
    Friend ReadOnly Property damage As damage
        Get
            Return _damage
        End Get
    End Property

    Friend Overrides Function UseCombat(ByRef target As icombatant) As Boolean
        If TypeOf target Is ship = False Then Return False
        If MyBase.UseCombat(ship) = False Then Return False

        Dim targetShip As ship = CType(target, ship)
        If _isCarrier = True Then
            targetShip.addInterceptor(ship, New interceptor(interceptorName, ship, target, _damage))
        Else
            targetShip.addDamage(ship, _damage)
        End If
        Return True
    End Function

    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Weapon"
        End Get
    End Property
End Class
