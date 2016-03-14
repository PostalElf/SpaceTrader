Public Class crew
    Friend Shared Function build() As crew
        Dim crew As New crew
        With crew
            If coinFlip() = True Then .gender = True Else .gender = eGender.Female
            If .gender = eGender.Male Then .nameFirst = getRandomAndRemove(namesFirstMale, "data/namesFirstMale.txt") Else .nameFirst = getRandomAndRemove(namesFirstFemale, "data/namesFirstFemale.txt")
            .nameLast = getRandomAndRemove(namesLast, "data/namesLast.txt")
        End With
        Return crew
    End Function
    Private Shared namesFirstMale As New List(Of String)
    Private Shared namesFirstFemale As New List(Of String)
    Private Shared namesLast As New List(Of String)

    Friend crewQuarters As hcCrewQuarters
    Private nameFirst As String
    Private nameLast As String
    Friend ReadOnly Property name As String
        Get
            Return nameFirst & " " & nameLast
        End Get
    End Property
    Private gender As eGender
End Class
