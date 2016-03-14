Public Class hcCrewQuarters
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aCrewMax As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        crewMax = aCrewMax
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return crewOccupied & "/" & crewMax
    End Function
    Friend Overrides Sub tickTravel()
        crewTick()
    End Sub
    Friend Overrides Sub tickIdle()
        crewTick()
    End Sub

    Private crewMax As Integer
    Private ReadOnly Property crewOccupied As Integer
        Get
            Return _crewList.Count
        End Get
    End Property
    Friend ReadOnly Property crewEmpty As Integer
        Get
            Return crewMax - crewOccupied
        End Get
    End Property
    Private _crewList As New List(Of crew)
    Friend ReadOnly Property crewList As List(Of crew)
        Get
            Return _crewList
        End Get
    End Property
    Private crewStarvation As Boolean = False
    Friend Sub addCrew(ByRef crew As crew)
        _crewList.Add(crew)
        crew.crewQuarters = Me
    End Sub
    Friend Sub removeCrew(ByRef crew As crew)
        If _crewList.Contains(crew) = False Then Exit Sub
        crew.crewQuarters = Nothing
        _crewList.Remove(crew)
    End Sub
    Private Sub crewTick()
        For n = 1 To crewOccupied
            If useResource() = False Then
                crewStarvation = True
                Exit Sub
            End If
        Next
        crewStarvation = False
    End Sub
End Class
