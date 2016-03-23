Public Class hcRepairer
    Inherits hullComponent
    Public Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aDefenceType As eDefenceType, ByVal aValue As Integer, ByVal aEnergyCost As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        defType = aDefenceType
        value = aValue
        energyCost = aEnergyCost
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return "Recharges " & value & " Shields"
    End Function

    Private energyCost As Integer
    Private defType As eDefenceType
    Private value As Integer
    Friend Sub Use()
        If crewable.isManned = False Then
            Console.WriteLine(name & " is not crewed!")
            Console.ReadKey()
            Exit Sub
        End If
        If ship.addEnergyCheck(-energyCost) = False Then
            Console.WriteLine("Insufficient energy!")
            Console.ReadKey()
            Exit Sub
        End If
        If useResource() = False Then
            Console.WriteLine("Insufficient resources!")
            Console.ReadKey()
            Exit Sub
        End If

        ship.addEnergy(-energyCost)
        ship.repair(eDefenceType.Shields, value)
    End Sub

    Friend crewable As New shcCrewable(Me)
    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Tactical Shield Charger"
        End Get
    End Property
End Class
