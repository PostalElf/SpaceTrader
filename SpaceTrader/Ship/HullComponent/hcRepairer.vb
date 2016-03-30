Public Class hcRepairer
    Inherits hullComponent
    Public Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aDefenceType As eDefenceType, ByVal aValue As Integer, ByVal aEnergyCost As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        defType = aDefenceType
        value = aValue
        _energyCost = aEnergyCost
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return "Recharges " & value & " Shields"
    End Function

    Private defType As eDefenceType
    Private value As Integer

    Friend Overrides Function UseCombat(ByRef target As iCombatant) As Boolean
        If MyBase.UseCombat(target) = False Then Return False
        Use()
        Return True
    End Function
    Friend Overrides Function UseCombatCheck(ByRef target As iCombatant) As Boolean
        If UseCheck() = False Then Return False Else Return MyBase.UseCombatCheck(target)
    End Function
    Private Sub Use()
        ship.repair(defType, value)
    End Sub
    Private Function UseCheck() As Boolean
        If defType <> eDefenceType.Shields AndAlso defType <> eDefenceType.Armour Then Return False
        Return True
    End Function

    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Repairer"
        End Get
    End Property
End Class
