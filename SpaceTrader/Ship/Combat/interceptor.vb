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
    Friend Sub destroy() Implements iCombatant.destroy
        target.removeInterceptor(Me)
        parent = Nothing
        target = Nothing
        damage = Nothing
    End Sub
    Friend Sub addAlert(ByVal title As String, ByVal text As String, ByVal priority As Integer) Implements iCombatant.addAlert
        parent.player.addAlert(title, text, priority)
    End Sub
End Class
