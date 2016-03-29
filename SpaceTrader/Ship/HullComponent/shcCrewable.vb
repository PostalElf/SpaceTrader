Public Class shcCrewable
    Friend Sub New(ByRef hc As hullComponent)
        _hullComponent = hc
    End Sub
    Friend Sub SetProperties(ByVal min As Integer, ByVal max As Integer)
        crewMin = min
        crewMax = max
    End Sub

    Private _hullComponent As hullComponent
    Friend ReadOnly Property hullComponent As hullComponent
        Get
            Return _hullComponent
        End Get
    End Property
    Private _crewList As New List(Of crew)
    Friend ReadOnly Property crewList As List(Of crew)
        Get
            Return _crewList
        End Get
    End Property
    Private crewMin As Integer
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
    Friend ReadOnly Property isManned As Boolean
        Get
            If crewOccupied >= crewMin Then Return True Else Return False
        End Get
    End Property

    Friend Sub assignCrewBest()
        Dim idlers As List(Of crew) = _hullComponent.ship.getCrews(True)
        If idlers.Count = 0 Then Exit Sub
        Dim p As crew = idlers(rng.Next(idlers.Count))
        assignCrew(p)
    End Sub
    Friend Sub assignCrew(ByRef crew As crew)
        _crewList.Add(crew)
        crew.crewAssignment = Me
    End Sub
    Friend Function assignCrewCheck(ByRef crew As crew) As Boolean
        If crewEmpty < 1 Then Return False
        If crew.crewAssignment Is Nothing = False Then Return False
        If _crewList.Contains(crew) Then Return False

        Return True
    End Function
    Friend Sub unassignCrew(ByRef crew As crew)
        If _crewList.Contains(crew) = False Then Exit Sub
        crew.crewAssignment = Nothing
        _crewList.Remove(crew)
    End Sub
    Friend Sub unassignCrewAll()
        For n = _crewList.Count - 1 To 0 Step -1
            unassignCrew(_crewList(n))
        Next
    End Sub
End Class
