Public Class saleable
    Friend Sub New(ByVal aName As String, ByVal tier As Integer, ByVal cost As Integer, ByVal type As String, ByRef aData As Queue(Of String))
        _name = aName
        _saleTier = tier
        _saleCost = cost
        _type = type
        data = aData

        Select Case saleTier
            Case 1 : _saleTimer = 5
            Case 2 : _saleTimer = 15
            Case 3 : _saleTimer = 25
            Case 4 : _saleTimer = 30
            Case Else : _saleTimer = 10
        End Select
    End Sub
    Friend Sub New(ByRef template As saleable)
        Me.New(template.name, template.saleTier, template.saleCost, template._type, template.data)
    End Sub
    Friend Shared Function buildRandom(ByVal tier As Integer, ByVal service As eService, Optional ByRef r As Random = Nothing) As saleable
        If r Is Nothing Then r = rng
        Dim data As List(Of saleable) = allSaleables(tier)(service)
        If data.Count = 0 Then Return Nothing

        Dim roll As Integer = r.Next(data.Count - 1)
        Dim template As saleable = data(roll)
        Return New saleable(template)
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

            Dim s As New saleable(name, tier, cost, type, q)
            Dim es As eService = s.service
            total(tier)(es).Add(s)
        Next

        Return total
    End Function

    Public Overrides Function ToString() As String
        Return name
    End Function
    Friend ReadOnly Property consoleDescription() As String
        Get
            Dim o = unpack()
            If TypeOf o Is hullComponent Then
                Dim hc As hullComponent = CType(o, hullComponent)
                Return hc.consoleDescription()
            End If

            Return Nothing
        End Get
    End Property

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
    Private _type As String
    Friend ReadOnly Property service As eService
        Get
            Return constants.getServiceFromTypeString(_type)
        End Get
    End Property
    Friend parentServices As Dictionary(Of eService, saleable)
    Private data As Queue(Of String)

    Friend Function unpack() As Object
        If service <> Nothing Then Return hullComponent.build(data)
        Return Nothing
    End Function

    Friend Sub tickSale()
        _saleTimer -= 1
        If _saleTimer <= 0 Then expire()
    End Sub
    Friend Sub expire()
        If parentServices Is Nothing = False Then
            parentServices(service) = Nothing
            parentServices = Nothing
        End If

        _saleTimer = 0
        data = Nothing
    End Sub
End Class
