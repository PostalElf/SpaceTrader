Public Class hcDefence
    Inherits hullComponent
    Implements ihcCrewable
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aType As eDefenceType, ByVal aValue As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        _defType = aType
        _value = aValue
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return withSign(value) & " " & defType.ToString
    End Function
    Friend Overrides ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)(MyBase.alarms)
            If crewable.isManned = False Then total.Add("Requires crew member(s).")
            Return total
        End Get
    End Property

    Private _defType As eShipType
    Friend ReadOnly Property defType As eDefenceType
        Get
            Return _defType
        End Get
    End Property
    Private _value As Integer
    Friend ReadOnly Property value As Integer
        Get
            If crewable.isManned = True Then Return _value Else Return 0
        End Get
    End Property

    Friend Property crewable As New shcCrewable(Me) Implements ihcCrewable.crewable
End Class
