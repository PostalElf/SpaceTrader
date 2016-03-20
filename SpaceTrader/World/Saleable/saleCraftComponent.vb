Public Class saleCraftComponent
    Inherits saleable
    Public Sub New(ByVal aName As String, ByVal tier As Integer, ByVal cost As Integer)
        MyBase.New(aName, tier, cost, Nothing)
    End Sub
    Public Sub New(ByVal template As saleCraftComponent)
        MyBase.New(template)
    End Sub
    Private Shared allSaleCraftComponents As Dictionary(Of Integer, List(Of saleCraftComponent)) = buildAllSaleables()
    Private Shared Function buildAllSaleables() As Dictionary(Of Integer, List(Of saleCraftComponent))
        Dim total As New Dictionary(Of Integer, List(Of saleCraftComponent))
        For tier = 1 To 4
            total.Add(tier, New List(Of saleCraftComponent))
        Next

        Dim inputlist As List(Of Queue(Of String)) = bracketFilegetAll("data/craftcomponents.txt")
        For Each q As Queue(Of String) In inputlist
            Dim tier As Integer
            Dim cost As Integer
            Dim qTemp As New Queue(q)

            Dim name As String = qTemp.Dequeue
            While qTemp.Count > 0
                Dim line As String = qTemp.Dequeue
                Dim ln As String() = line.Split(":")
                ln(1) = ln(1).Trim
                Select Case ln(0).ToLower
                    Case "tier" : tier = CInt(ln(1))
                    Case "cost" : cost = CInt(ln(1))
                End Select
            End While

            Dim s As New saleCraftComponent(name, tier, cost)
            total(tier).Add(s)
        Next
        Return total
    End Function
    Friend Shared ReadOnly Property allSaleCraftComponentList As List(Of saleCraftComponent)
        Get
            Dim total As New List(Of saleCraftComponent)
            For Each kvp In allSaleCraftComponents
                total.AddRange(kvp.Value)
            Next
            Return total
        End Get
    End Property
    Friend Shared Function buildRandom(ByVal tier As Integer, Optional ByRef r As Random = Nothing) As saleCraftComponent
        If r Is Nothing Then r = rng
        Dim data As List(Of saleCraftComponent) = allSaleCraftComponents(tier)
        If data.Count = 0 Then Return Nothing

        Dim roll As Integer = r.Next(data.Count)
        Dim template As saleable = data(roll)
        Return New saleCraftComponent(template)
    End Function

    Public Overrides Function ToString() As String
        Return name
    End Function
    Friend Overrides ReadOnly Property consoleDescription As String
        Get
            Return name
        End Get
    End Property

    Friend Shared Function getDefaultPrice(ByVal str As String) As Integer
        For Each item In allSaleCraftComponentList
            If item.name = str Then Return item.saleCost
        Next
        Return -1
    End Function

    Friend parentSaleList As List(Of saleCraftComponent)

    Friend Overrides Function unpack() As Object
        Return name
    End Function
    Friend Overrides Sub expire()
        parentSaleList.Remove(Me)
        parentSaleList = Nothing
    End Sub
End Class
