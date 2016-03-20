Public MustInherit Class saleable
    Friend Sub New(ByVal aName As String, ByVal tier As Integer, ByVal cost As Integer, ByRef aData As Queue(Of String))
        _name = aName
        _saleTier = tier
        _saleCost = cost
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
        Me.New(template.name, template.saleTier, template.saleCost, template.data)
    End Sub

    Public Overrides Function ToString() As String
        Return name
    End Function
    Friend MustOverride ReadOnly Property consoleDescription() As String

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
    Protected data As Queue(Of String)

    Friend MustOverride Function unpack() As Object
    Friend Sub tickSale()
        _saleTimer -= 1
        If _saleTimer <= 0 Then expire()
    End Sub
    Friend MustOverride Sub expire()
End Class
