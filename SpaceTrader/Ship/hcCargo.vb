Public Class hcCargo
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aResource As eResource, ByVal aValue As Integer)
        MyBase.New(aName, aSize)
        resource = aResource
        value = aValue
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return withSign(value) & " " & resource.ToString & " capacity"
    End Function

    Friend resource As eResource
    Friend value As Integer
End Class
