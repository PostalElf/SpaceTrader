Public Class hcProducer
    Inherits hullComponent

    Private resourceProduction As eResource
    Private resourceProductionQty As Integer
    Private resourceProductionTimer As Integer
    Private resourceProductionTimerMax As Integer
    Private Sub resourceProductionTick()
        resourceProductionTimer += 1
        If resourceProductionTimer >= resourceProductionTimerMax Then
            resourceProductionTimer = 0
            ship.addResource(resourceProduction, resourceProductionQty)
        End If
    End Sub
End Class
