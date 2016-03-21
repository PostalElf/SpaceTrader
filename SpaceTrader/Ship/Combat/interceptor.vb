Public Class interceptor
    Implements iCombatant
    Friend Sub New(ByVal aName As String, ByRef aParent As ship, ByRef aTarget As ship, ByVal aDamage As damage)
        _name = aName
        parent = aParent
        target = aTarget
        damage = aDamage
    End Sub

    Private _name As String
    Friend ReadOnly Property name As String Implements iCombatant.name
        Get
            Return _name
        End Get
    End Property
    Private parent As ship
    Private target As ship
    Private damage As damage

    Friend Sub tickCombat()
        target.addDamage(Me, damage)
    End Sub
End Class
