Public Class faction
    Private _name As String
    Friend ReadOnly Property name As String
        Get
            Return _name
        End Get
    End Property

    Friend planets As New List(Of planet)

    Private _prosperityBase As Integer
    Friend ReadOnly Property prosperityBase As Integer
        Get
            Return _prosperityBase
        End Get
    End Property
    Private _militaryBase As Integer
    Friend ReadOnly Property militaryBase As Integer
        Get
            Return _militaryBase
        End Get
    End Property
End Class
