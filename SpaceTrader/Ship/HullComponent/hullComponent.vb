Public MustInherit Class hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer)
        _name = aName
        _size = aSize
    End Sub
    Public Overrides Function ToString() As String
        Return name & ": " & consoleDescription()
    End Function
    Friend Function consoleResourceDescription() As String
        If resourceSlot = Nothing Then Return Nothing
        Return "[" & resourceSlot.ToString & " " & resourceQtyRemaining & "%]"
    End Function
    Friend MustOverride Function consoleDescription() As String

    Protected _name As String
    Friend ReadOnly Property name As String
        Get
            Return _name
        End Get
    End Property
    Friend ship As ship
    Protected _size As Integer
    Friend ReadOnly Property size As Integer
        Get
            Return _size
        End Get
    End Property
    Friend Overridable Sub tickTravel()
        'handle in subclass if necessary
    End Sub
    Friend Overridable Sub tickIdle()
        'handle in subclass if necessary
    End Sub

    Private resourceSlot As eResource
    Private resourceQtyRemaining As Integer
    Private resourceQtyPerUse As Integer
    Friend Sub setResourceSlot(ByVal aResourceSlot As eResource, ByVal aResourceQtyPerUse As Integer)
        resourceSlot = aResourceSlot
        resourceQtyPerUse = aResourceQtyPerUse
    End Sub
    Friend Sub loadResource()
        If resourceSlot = Nothing Then Exit Sub
        If ship.addResourceCheck(resourceSlot, -1) = False Then
            alert.Add("Load Failure", name & " was unable to load " & resourceSlot.ToString & " from the cargo bay.", 5)
            Exit Sub
        End If

        ship.addResource(resourceSlot, -1)
        resourceQtyRemaining += 100
    End Sub
    Friend Function useResource() As Boolean
        If resourceSlot = Nothing Then Return True
        If resourceQtyPerUse > resourceQtyRemaining Then
            alert.Add("Use Failure", name & " is out of " & resourceSlot.ToString & "!", 5)
            Return False
        End If

        resourceQtyRemaining -= resourceQtyPerUse
        Return True
    End Function
End Class
