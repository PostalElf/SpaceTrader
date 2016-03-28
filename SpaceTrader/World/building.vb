Public Class building
    Friend Sub New(ByVal aName As String, ByVal aCost As Integer, ByVal aProsperity As Integer, ByVal aMilitary As Integer, ByVal aInstability As Integer)
        name = aName
        cost = aCost
        prosperity = aProsperity
        military = aMilitary
        instability = aInstability
    End Sub
    Friend Sub New(ByVal template As building)
        With template
            name = .name
            cost = .cost
            prosperity = .prosperity
            military = .military
        End With
    End Sub
    Friend Shared Function build(ByVal buildingName As String) As building
        Dim q As Queue(Of String) = bracketFileget("data/buildings.txt", buildingName)
        Dim name As String = q.Dequeue
        Dim cost, prosperity, military, instability As Integer

        While q.Count > 0
            Dim l As String = q.Dequeue
            Dim ln As String() = l.Split(":")
            For n = 0 To ln.Count - 1
                ln(n) = ln(n).Trim
            Next

            Select Case ln(0).ToLower
                Case "cost" : cost = CInt(ln(1))
                Case "prosperity" : prosperity = CInt(ln(1))
                Case "military" : military = CInt(ln(1))
                Case "instability" : instability = CInt(ln(1))
            End Select
        End While

        Return New building(name, cost, prosperity, military, instability)
    End Function
    Friend Shared Function getBuildingPriceDefault(ByVal name As String) As Integer
        Dim building As building = build(name)
        If building Is Nothing Then Return -1
        Return building.cost
    End Function

    Friend name As String
    Friend cost As Integer
    Friend owner As player
    Friend planet As planet

    Friend prosperity As Integer
    Friend military As Integer
    Friend instability As Integer
End Class
