Public Class hcProducer
    Inherits hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aResourceProduction As eResource, ByVal aResourceProductionQty As Integer, ByVal aResourceProductionTimerMax As Integer)
        MyBase.New(aName, aSize)
        resourceProduction = aResourceProduction
        resourceProductionQty = aResourceProductionQty
        resourceProductionTimerMax = aResourceProductionTimerMax
    End Sub
    Friend Overrides Function consoleDescription() As String
        Return "produces " & resourceProductionQty & " " & resourceProduction.ToString & " per " & resourceProductionTimerMax
    End Function

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
