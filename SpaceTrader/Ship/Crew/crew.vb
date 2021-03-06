﻿Public Class crew
    Friend Shared Function build(ByVal race As eRace, Optional ByRef r As Random = Nothing) As crew
        If r Is Nothing Then r = rng

        Dim crew As New crew
        With crew
            ._race = race
            Select Case race
                Case eRace.Human
                    If coinFlip(r) = True Then .gender = True Else .gender = eGender.Female
                    If .gender = eGender.Male Then .nameFirst = getRandomAndRemove(namesFirstMale, "data/namesFirstMale.txt", r) Else .nameFirst = getRandomAndRemove(namesFirstFemale, "data/namesFirstFemale.txt", r)
                    .nameLast = getRandomAndRemove(namesLast, "data/namesLast.txt", r)

                Case eRace.Uplifted
                    .gender = eGender.Thing
                    .nameFirst = r.Next(65536).ToString("X")
                    .nameLast = r.Next(0, 16).ToString("X") & "x"
            End Select
        End With
        Return crew
    End Function
    Private Shared namesFirstMale As New List(Of String)
    Private Shared namesFirstFemale As New List(Of String)
    Private Shared namesLast As New List(Of String)

    Public Overrides Function ToString() As String
        Return name
    End Function

    Friend crewQuarters As hcCrewQuarters
    Friend crewAssignment As shcCrewable
    Private ReadOnly Property ship As ship
        Get
            If crewQuarters Is Nothing Then Return Nothing
            Return crewQuarters.ship
        End Get
    End Property

    Private nameFirst As String
    Private nameLast As String
    Friend ReadOnly Property name As String
        Get
            Select Case race
                Case eRace.Human : Return nameFirst & " " & nameLast
                Case eRace.Uplifted : Return nameLast & nameFirst
                Case Else : Return Nothing
            End Select
        End Get
    End Property
    Private gender As eGender
    Private _race As eRace
    Friend ReadOnly Property race As eRace
        Get
            Return _race
        End Get
    End Property

    Friend Sub destroy(Optional ByVal cause As String = "")
        If crewQuarters Is Nothing = False Then crewQuarters.removeCrew(Me)
        If crewAssignment Is Nothing = False Then crewAssignment.unassignCrew(Me)

        Select Case cause.ToLower
            Case "starvation" : ship.player.addAlert("Crew Dead", name & " has starved to death.", 1)
            Case Else : ship.player.addAlert("Crew Dead", name & " is dead.", 1)
        End Select
    End Sub
End Class
