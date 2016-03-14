Public Class hcJumpDrive
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aJumpSpeed As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        _jumpSpeed = aJumpSpeed
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return _jumpSpeed & " jump speed"
    End Function
    Friend Overrides Sub tickTravel()
        If ship.isJump = True Then
            _isActive = useResource()
        End If
    End Sub

    Private _isActive As Boolean
    Friend ReadOnly Property isActive As Boolean
        Get
            Return _isActive
        End Get
    End Property
    Private _jumpSpeed As Integer
    Friend ReadOnly Property jumpSpeed As Integer
        Get
            Return _jumpSpeed
        End Get
    End Property
End Class
