Public Class hcDefence
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aType As eDefenceType, ByVal aValue As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        type = aType
        value = aValue
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return withSign(value) & " " & type.ToString
    End Function

    Friend type As eDefenceType
    Friend value As Integer
End Class
