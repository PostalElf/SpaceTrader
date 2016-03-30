Public Class hcProducer
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aResourceProduction As eResource, ByVal aResourceProductionTimerMax As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        resourceProduction = aResourceProduction
        resourceProductionTimerMax = aResourceProductionTimerMax
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return "1 " & resourceProduction.ToString & " per " & resourceProductionTimerMax & " ticks"
    End Function

    Friend Overrides Sub tickTravel()
        resourceProductionTick()
    End Sub
    Friend Overrides Sub tickIdle()
        resourceProductionTick()
    End Sub

    Private resourceProduction As eResource
    Private resourceProductionTimer As Integer
    Private resourceProductionTimerMax As Integer
    Private Sub resourceProductionTick()
        If crewable.isManned = False Then Exit Sub
        If useResource() = False Then Exit Sub

        resourceProductionTimer += 1
        If resourceProductionTimer >= resourceProductionTimerMax Then
            resourceProductionTimer = 0
            ship.player.AddAlert("Production", name & " has produced a pod of " & resourceProduction.ToString & ".", 7)
            ship.addResource(resourceProduction, 1)
        End If
    End Sub

    Friend Overrides ReadOnly Property typeString As String
        Get
            Return "Producer"
        End Get
    End Property
End Class
