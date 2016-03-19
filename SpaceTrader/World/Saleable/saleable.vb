Public MustInherit Class saleable
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
    Protected _saleTimer As Integer
    Friend ReadOnly Property saleTimer As Integer
        Get
            Return _saleTimer
        End Get
    End Property
    Private _type As String
    Friend ReadOnly Property service As eHcCategory
        Get
            Return constants.getServiceFromTypeString(_type)
        End Get
    End Property
    Protected data As Queue(Of String)

    Friend MustOverride Function unpack() As Object
    Friend Sub tickSale()
        _saleTimer -= 1
        If _saleTimer <= 0 Then expire()
    End Sub
    Friend MustOverride Sub expire()
End Class
