Public Class saleHullcomponent
    Inherits saleable
    Friend Sub New(ByVal aName As String, ByVal tier As Integer, ByVal cost As Integer, ByVal type As String, ByRef aData As Queue(Of String))
        MyBase.New(aName, tier, cost, type, aData)
    End Sub
    Public Sub New(ByVal template As saleable)
        MyBase.New(template)
    End Sub
    Private Shared allSaleHullcomponents As Dictionary(Of Integer, Dictionary(Of eHcCategory, List(Of saleable))) = buildAllSaleables()
    Private Shared Function buildAllSaleables() As Dictionary(Of Integer, Dictionary(Of eHcCategory, List(Of saleable)))
        'initialise dictionary
        Dim total As New Dictionary(Of Integer, Dictionary(Of eHcCategory, List(Of saleable)))
        For tier = 1 To 4
            total.Add(tier, New Dictionary(Of eHcCategory, List(Of saleable)))
            For Each es In constants.saleHullComponentArray
                total(tier).Add(es, New List(Of saleable))
            Next
        Next

        Dim input As List(Of Queue(Of String)) = bracketFilegetAll("data/hullcomponents.txt")
        For Each q As Queue(Of String) In input
            Dim name As String = ""
            Dim type As String = ""
            Dim tier, cost As Integer

            Dim qTemp As New Queue(q)
            name = qTemp.Dequeue
            While qTemp.Count > 0
                Dim line As String = qTemp.Dequeue
                Dim ln As String() = line.Split(":")
                ln(1) = ln(1).Trim
                Select Case ln(0).ToLower
                    Case "type" : type = ln(1)
                    Case "tier" : tier = CInt(ln(1))
                    Case "cost" : cost = CInt(ln(1))
                End Select
            End While

            Dim s As New saleHullcomponent(name, tier, cost, type, q)
            Dim es As eHcCategory = s.service
            total(tier)(es).Add(s)
        Next

        Return total
    End Function
    Friend Shared Function buildRandom(ByVal tier As Integer, ByVal service As eHcCategory, Optional ByRef r As Random = Nothing) As saleable
        If r Is Nothing Then r = rng
        Dim data As List(Of saleable) = allSaleHullcomponents(tier)(service)
        If data.Count = 0 Then Return Nothing

        Dim roll As Integer = r.Next(data.Count - 1)
        Dim template As saleable = data(roll)
        Return New saleHullcomponent(template)
    End Function

    Friend parentServices As Dictionary(Of eHcCategory, saleHullcomponent)
    Friend Overrides Function unpack() As Object
        Return hullComponent.build(data)
    End Function
    Friend Overrides Sub expire()
        If parentServices Is Nothing = False Then
            parentServices(service) = Nothing
            parentServices = Nothing
        End If

        _saleTimer = 0
        data = Nothing
    End Sub
End Class
