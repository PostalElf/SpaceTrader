Public Class hcWeapon
    Inherits hullComponent
    Implements ihcCrewable
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aEnergyCost As Integer, _
                   ByVal aDamageType As eDamageType, ByVal aAccuracy As Integer, ByVal aDamageFull As Integer, ByVal aDamageGlancing As Integer, ByVal digitalPayload As eDigitalAttack, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)

        _energyCost = aEnergyCost
        If aDamageType = eDamageType.Interceptors Then
            isCarrier = True
            _damage = New damage(eDamageType.Ballistic, aAccuracy, aDamageFull, aDamageGlancing, digitalPayload)
        Else
            isCarrier = False
            _damage = New damage(aDamageType, aAccuracy, aDamageFull, aDamageGlancing, digitalPayload)
        End If
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

    Private _energyCost As Integer
    Friend ReadOnly Property energyCost As Integer
        Get
            Return _energyCost
        End Get
    End Property
    Private isCarrier As Boolean
    Friend interceptorName As String
    Private _damage As damage
    Friend ReadOnly Property damage As damage
        Get
            Return _damage
        End Get
    End Property
    Friend Sub attack(ByRef target As ship)
        If crewable.isManned = False Then
            Console.WriteLine(name & " is not crewed!")
            Console.ReadKey()
            Exit Sub
        End If
        If ship.addEnergyCheck(-energyCost) = False Then
            Console.WriteLine("Insufficient energy!")
            Console.ReadKey()
            Exit Sub
        End If
        If useResource() = False Then Exit Sub

        ship.addEnergy(-energyCost)
        If isCarrier = True Then
            ship.player.addAlert("Attack", ship.name & " launches an interceptor at " & target.name & ".", 2)
            target.addInterceptor(ship, New interceptor(interceptorName, ship, target, _damage))
        Else
            target.addDamage(ship, _damage)
        End If
    End Sub

    Friend Property crewable As New shcCrewable(Me) Implements ihcCrewable.crewable
    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Weapon"
        End Get
    End Property
End Class
