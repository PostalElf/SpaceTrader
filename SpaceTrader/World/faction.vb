Public Class faction
    Friend Shared Function build(ByRef r As Random) As faction
        Dim faction As New faction
        With faction
            ._name = "Faction #" & r.Next(1000, 10000)
            ._prosperityBase = 100
            ._militaryBase = 100
        End With
        Return faction
    End Function

    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftlen As Integer = 12

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Prosperity:", ftlen) & prosperityBase)
        Console.WriteLine(indd & fakeTab("Military:", ftlen) & militaryBase)
        Console.WriteLine(indd & "Planets:")
        For Each planet In planets
            Console.WriteLine(inddd & planet.name)
        Next
    End Sub
    Public Overrides Function ToString() As String
        Return name
    End Function

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
    Friend Sub addProsperity(ByVal value As Integer)
        _prosperityBase = constrain(_prosperityBase + value, 1, 200)
    End Sub
    Private _militaryBase As Integer
    Friend ReadOnly Property militaryBase As Integer
        Get
            Return _militaryBase
        End Get
    End Property
    Friend Sub addMilitary(ByVal value As Integer)
        _militaryBase = constrain(_militaryBase + value, 1, 200)
    End Sub
End Class
