Public Class hcJumpDrive
    Inherits hullComponent
    Implements ihcCrewable
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aJumpSpeed As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        _jumpSpeed = aJumpSpeed
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return withSign(jumpSpeed) & " jump speed"
    End Function
    Friend Overrides ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)(MyBase.alarms)
            If crewable.isManned = False Then total.Add("Requires crew member(s).")
            Return total
        End Get
    End Property

    Friend Overrides Sub tickTravel()
        _isActive = useResource()
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
            If crewable.isManned = True Then Return _jumpSpeed Else Return 0
        End Get
    End Property
    Friend Overrides Function loadResource() As Boolean
        Dim result As Boolean = MyBase.loadResource()
        _isActive = result
        Return result
    End Function

    Friend Property crewable As New shcCrewable(Me) Implements ihcCrewable.crewable
    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "JumpDrive"
        End Get
    End Property
End Class
