Public Interface iCombatant
    ReadOnly Property name As String

    Sub destroy()
    Sub addAlert(ByVal title As String, ByVal text As String, ByVal priority As Integer)
End Interface
