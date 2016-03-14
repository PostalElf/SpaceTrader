Public Class hcWeapon
    Inherits hullComponent

    Private damage As Integer
    Private damageType As eDamageType
    Private Sub attack(ByRef target As ship)
        If useResource() = False Then Exit Sub

        alert.Add("Attack", ship.name & " deals " & damage & " " & damageType.ToString & " damage to " & target.name & ".", 2)
        target.addDamage(damage, damageType)
    End Sub
End Class
