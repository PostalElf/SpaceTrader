Public Class hcEngine
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aSpeed As Integer, ByVal aDodge As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        _speed = aSpeed
        _dodge = aDodge
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return withSign(speed) & " sublight speed, " & withSign(dodge) & " dodge"
    End Function
    Friend Overrides Sub tickTravel()
        If ship.travelByJump = False Then
            _isActive = useResource()
        End If
    End Sub

    Private _isActive As Boolean
    Friend ReadOnly Property isActive As Boolean
        Get
            Return _isActive
        End Get
    End Property
    Private _speed As Integer
    Friend ReadOnly Property speed As Integer
        Get
            Return _speed
        End Get
    End Property
    Private _dodge As Integer
    Friend ReadOnly Property dodge As Integer
        Get
            Return _dodge
        End Get
    End Property
    Friend Overrides Function loadResource() As Boolean
        Dim result As Boolean = MyBase.loadResource()
        _isActive = result
        Return result
    End Function

    Friend crewable As New shcCrewable(Me)
End Class
