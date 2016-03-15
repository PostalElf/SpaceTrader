Public MustInherit Class hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aResourceSlot As eResource, ByVal aResourceQtyPerUse As Integer)
        _name = aName
        _size = aSize
        If aResourceSlot <> Nothing AndAlso aResourceQtyPerUse <> 0 Then setResourceSlot(aResourceSlot, aResourceQtyPerUse)
    End Sub
    Public Overrides Function ToString() As String
        Return name & ": " & consoleDescription()
    End Function
    Friend Function consoleResourceDescription() As String
        If resourceSlot = Nothing Then Return Nothing
        Return "[" & resourceSlot.ToString & " " & resourceQtyRemaining & "%]"
    End Function
    Friend MustOverride Function consoleDescription() As String
    Friend Overridable ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)
            If resourceSlot <> Nothing AndAlso resourceQtyRemaining = 0 Then total.Add("No " & resourceSlot.ToString & " remaining.")
            Return total
        End Get
    End Property

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
    Friend Overridable Function loadResource() As Boolean
        If resourceSlot = Nothing Then Return True
        If resourceQtyRemaining >= 100 Then Return False
        If ship.addResourceCheck(resourceSlot, -1) = False Then
            alert.Add("Load Failure", name & " was unable to load " & resourceSlot.ToString & " from the cargo hold.", 5)
            Return False
        End If

        ship.addResource(resourceSlot, -1)
        resourceQtyRemaining += 100
        alert.Add("Load", name & " loaded a pod of " & resourceSlot.ToString & " from the cargo hold.", 7)
        Return True
    End Function
    Protected Function useResource() As Boolean
        If resourceSlot = Nothing Then Return True
        If resourceQtyPerUse > resourceQtyRemaining Then
            alert.Add("Use Failure", name & " is out of " & resourceSlot.ToString & "!", 5)
            Return False
        End If

        resourceQtyRemaining -= resourceQtyPerUse
        Return True
    End Function
End Class
