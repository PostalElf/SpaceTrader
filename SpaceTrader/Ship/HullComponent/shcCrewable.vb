Public Class shcCrewable
    Friend Sub New(ByRef hc As hullComponent)
        hullComponent = hc
    End Sub

    Private hullComponent As hullComponent
    Private _crewList As New List(Of crew)
    Friend ReadOnly Property crewList As List(Of crew)
        Get
            Return _crewList
        End Get
    End Property
    Private crewMax As Integer
    Private ReadOnly Property crewOccupied
        Get
            Return _crewList.Count
        End Get
    End Property
    Private ReadOnly Property crewEmpty
        Get
            Return crewMax - crewOccupied
        End Get
    End Property
    Friend Sub assignCrew(ByRef crew As crew)
        _crewList.Add(crew)
        crew.crewAssignment = Me
    End Sub
    Friend Function assignCrewCheck(ByRef crew As crew) As Boolean
        If crewEmpty < 1 Then Return False
        If _crewList.Contains(crew) Then Return False

        Return True
    End Function
    Friend Sub unassignCrew(ByRef crew As crew)
        If _crewList.Contains(crew) = False Then Exit Sub
        crew.crewAssignment = Nothing
        _crewList.Remove(crew)
    End Sub
End Class
