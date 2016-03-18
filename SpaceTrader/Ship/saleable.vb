Public Class saleable
    Friend Sub New(ByVal aName As String, ByVal tier As Integer, ByVal cost As Integer, ByVal aService As eService, ByRef aData As Queue(Of String))
        _name = aName
        _saleTier = tier
        _saleCost = cost
        _service = aService
        _saleTimer = 10
    End Sub
    Friend Shared Function buildRandom(ByVal tier As Integer, ByVal service As eService, Optional ByRef r As Random = Nothing) As saleable
        If r Is Nothing Then r = rng
        Dim data As List(Of saleable) = allSaleables(tier)(service)
        If data.Count = 0 Then Return Nothing

        Dim roll As Integer = r.Next(data.Count - 1)
        Return data(roll)
    End Function
    Private Shared allSaleables As Dictionary(Of Integer, Dictionary(Of eService, List(Of saleable))) = buildAllSaleables()
    Private Shared Function buildAllSaleables() As Dictionary(Of Integer, Dictionary(Of eService, List(Of saleable)))
        'initialise dictionary
        Dim total As New Dictionary(Of Integer, Dictionary(Of eService, List(Of saleable)))
        For tier = 1 To 4
            total.Add(tier, New Dictionary(Of eService, List(Of saleable)))
            For Each es In constants.serviceArray
                If es <> eService.Repair Then total(tier).Add(es, New List(Of saleable))
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

            Dim es As eService = constants.getServiceFromTypeString(type)
            Dim s As New saleable(name, tier, cost, es, q)
            total(tier)(es).Add(s)
        Next

        Return total
    End Function

    Public Overrides Function ToString() As String
        Return name
    End Function

    Private _name As String
    Friend ReadOnly Property name As String
        Get
            Return _name
        End Get
    End Property
    Private _saleTier As Integer
    Friend ReadOnly Property saleTier As Integer
        Get
            Return _saleTier
        End Get
    End Property
    Private _saleCost As Integer
    Friend ReadOnly Property saleCost As Integer
        Get
            Return _saleCost
        End Get
    End Property
    Private _saleTimer As Integer
    Friend ReadOnly Property saleTimer As Integer
        Get
            Return _saleTimer
        End Get
    End Property
    Private _service As eService
    Friend ReadOnly Property service As eService
        Get
            Return _service
        End Get
    End Property
    Private data As Queue(Of String)

    Friend Function unpack() As hullComponent
        Return hullComponent.build(data)
    End Function

    Friend ReadOnly Property isExpired As Boolean
        Get
            If _saleTimer <= 0 Then Return True Else Return False
        End Get
    End Property
    Friend Sub tickSale()
        _saleTimer -= 1
    End Sub
End Class
