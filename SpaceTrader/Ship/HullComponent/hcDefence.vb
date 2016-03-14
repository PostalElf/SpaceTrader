Public Class hcDefence
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aType As eDefenceType, ByVal aValue As Integer)
        MyBase.New(aName, aSize)
        type = aType
        value = aValue
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return value & " " & type.ToString
    End Function

    Friend type As eDefenceType
    Friend value As Integer
End Class
