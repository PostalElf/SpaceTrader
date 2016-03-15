Public Class hcProducer
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aResourceProduction As eResource, ByVal aResourceProductionQty As Integer, ByVal aResourceProductionTimerMax As Integer, _
                   Optional ByVal aResourceSlot As eResource = Nothing, Optional ByVal aResourceQtyPerUse As Integer = 0)
        MyBase.New(aName, aSize, aResourceSlot, aResourceQtyPerUse)
        resourceProduction = aResourceProduction
        resourceProductionQty = aResourceProductionQty
        resourceProductionTimerMax = aResourceProductionTimerMax
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return withSign(resourceProductionQty) & " " & resourceProduction.ToString & " per " & resourceProductionTimerMax & " ticks"
    End Function
    Friend Overrides Sub tickTravel()
        resourceProductionTick()
    End Sub
    Friend Overrides Sub tickIdle()
        resourceProductionTick()
    End Sub

    Private resourceProduction As eResource
    Private resourceProductionQty As Integer
    Private resourceProductionTimer As Integer
    Private resourceProductionTimerMax As Integer
    Private Sub resourceProductionTick()
        If useResource() = False Then Exit Sub

        resourceProductionTimer += 1
        If resourceProductionTimer >= resourceProductionTimerMax Then
            resourceProductionTimer = 0
            alert.Add("Production", name & " has produced a pod of " & resourceProduction.ToString & ".", 7)
            ship.addResource(resourceProduction, resourceProductionQty)
        End If
    End Sub
End Class
