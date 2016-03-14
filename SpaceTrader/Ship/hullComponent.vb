Public MustInherit Class hullComponent
    Friend Sub consoleReport(ByVal indent As Integer, Optional ByVal prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix

        Console.WriteLine(ind & name)
    End Sub

    Private name As String
    Friend ship As ship
    Protected _size As Integer
    Friend ReadOnly Property size As Integer
        Get
            Return _size
        End Get
    End Property

    Private resourceSlot As eResource
    Private resourceQtyRemaining As Integer
    Private resourceQtyPerUse As Integer
    Friend Sub loadResource()
        If resourceSlot = Nothing Then Exit Sub
        If ship.addResourceCheck(resourceSlot, -1) = False Then
            alert.Add("Load Failure", name & " was unable to load " & resourceSlot.ToString & " from the cargo bay.", 5)
            Exit Sub
        End If

        ship.addResource(resourceSlot, -1)
        resourceQtyRemaining += 100
    End Sub
    Friend Function useResource() As Boolean
        If resourceSlot = Nothing Then Return True
        If resourceQtyPerUse > resourceQtyRemaining Then
            alert.Add("Use Failure", name & " is out of " & resourceSlot.ToString & "!", 5)
            Return False
        End If

        resourceQtyRemaining -= resourceQtyPerUse
        Return True
    End Function
End Class
