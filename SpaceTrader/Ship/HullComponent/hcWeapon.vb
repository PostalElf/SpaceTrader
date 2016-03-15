Public Class hcWeapon
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aDamage As Integer, ByVal aDamageType As eDamageType, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        damage = aDamage
        damageType = aDamageType
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return damage & " " & damageType.ToString & " damage"
    End Function
    Friend Overrides ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)(MyBase.alarms)
            If crewable.isManned = False Then total.Add("Requires crew member(s).")
            Return total
        End Get
    End Property

    Private damage As Integer
    Private damageType As eDamageType
    Private Sub attack(ByRef target As ship)
        If crewable.isManned = False Then Exit Sub
        If useResource() = False Then Exit Sub

        alert.Add("Attack", ship.name & " deals " & damage & " " & damageType.ToString & " damage to " & target.name & ".", 2)
        target.addDamage(damage, damageType)
    End Sub

    Friend crewable As New shcCrewable(Me)
End Class
