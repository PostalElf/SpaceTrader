Public Class player
    Private _credits As Integer
    Friend ReadOnly Property credits As Integer
        Get
            Return _credits
        End Get
    End Property
    Friend Sub addCredits(ByVal value As Integer)
        _credits += value
    End Sub
    Friend Function addCreditsCheck(ByVal value As Integer) As Boolean
        If value > _credits Then Return False
        Return True
    End Function
End Class
