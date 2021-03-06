﻿Public Class hcCrewQuarters
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aCrewMax As Integer, ByVal aCrewRace As eRace, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        crewMax = aCrewMax
        crewRace = aCrewRace
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return crewOccupied & "/" & crewMax
    End Function
    Friend Overrides Sub tickTravel()
        crewEatTick()
    End Sub
    Friend Overrides Sub tickIdle()
        crewEatTick()
    End Sub

    Private crewRace As eRace
    Private crewMax As Integer
    Private ReadOnly Property crewOccupied As Integer
        Get
            Return _crewList.Count
        End Get
    End Property
    Private ReadOnly Property crewEmpty As Integer
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
    Friend Sub addCrew(ByRef crew As crew)
        _crewList.Add(crew)
        crew.crewQuarters = Me
        ship.player.addAlert("Berthing", crew.name & " is now berthing in " & name & ".", 9)
    End Sub
    Friend Function addCrewCheck(ByRef crew As crew) As Boolean
        If crewList.Contains(crew) Then Return False
        If crewEmpty < 1 Then Return False
        If crewRace <> Nothing AndAlso crew.race <> crewRace Then Return False

        Return True
    End Function
    Friend Sub removeCrew(ByRef crew As crew)
        If _crewList.Contains(crew) = False Then Exit Sub
        crew.crewQuarters = Nothing
        _crewList.Remove(crew)
    End Sub
    Private Sub crewEatTick()
        Dim crewStarvation As Boolean = False
        For n = 1 To crewOccupied
            If useResource() = False Then crewStarvation = True
        Next

        If crewStarvation = True Then
            ship.player.addAlert("Starvation", "The crew in " & name & " is starving.", 5)
            For n = _crewList.Count - 1 To 0 Step -1
                Dim c As crew = _crewList(n)
                If percentRoll(65) = True Then c.destroy("starvation")
            Next
        End If
    End Sub

    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "CrewQuarters"
        End Get
    End Property
End Class
