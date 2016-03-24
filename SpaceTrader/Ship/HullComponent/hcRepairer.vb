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

        ship.repair(eDefenceType.Shields, value)
        Return True
    End Function
    Friend Overrides Function UseCombatCheck(ByRef target As iCombatant) As Boolean
        If defType <> eDefenceType.Shields AndAlso defType <> eDefenceType.Armour Then Return False
        If TypeOf target Is ship = False Then Return False
        Return MyBase.UseCombatCheck(target)
    End Function

    Friend Overrides ReadOnly Property typeString As String
        Get
            Select Case defType
                Case eDefenceType.Armour : Return "Armour Repairer"
                Case eDefenceType.Shields : Return "Shields Recharger"
                Case Else
                    MsgBox("hcRepairer.typestring: invalid defType")
                    Return Nothing
            End Select
        End Get
    End Property
End Class
