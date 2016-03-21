Public MustInherit Class hullComponent
    Friend Sub New(ByVal aName As String, ByVal aSize As Integer, ByVal aResourceSlot As eResource, ByVal aResourceQtyPerUse As Integer)
        _name = aName
        _size = aSize
        If aResourceSlot <> Nothing AndAlso aResourceQtyPerUse <> 0 Then setResourceSlot(aResourceSlot, aResourceQtyPerUse)
    End Sub
    Friend Shared Function build(ByVal targetName As String) As hullComponent
        Dim inputList As Queue(Of String) = bracketFileget("data/hullcomponents.txt", targetName)
        If inputList Is Nothing Then Return Nothing
        Return build(inputList)
    End Function
    Friend Shared Function build(ByVal inputlist As Queue(Of String)) As hullComponent
        'get name from head of queue
        Dim targetName As String = inputlist.Dequeue


        'initialise variables
        Dim type As String = ""
        Dim blueprint As List(Of String) = Nothing
        Dim size As Integer
        Dim value As Integer
        Dim crewRace As eRace
        Dim crewMax As Integer
        Dim damage As Integer
        Dim damageType As eDamageType
        Dim defenceType As eDefenceType
        Dim speed As Integer
        Dim dodge As Integer
        Dim resource As eResource
        Dim resourceProductionTimer As Integer
        Dim resourceSlot As eResource
        Dim resourceQtyPerUse As Integer
        Dim crewableMin As Integer
        Dim crewableMax As Integer


        While inputlist.Count > 0
            'split and trim
            Dim line As String = inputlist.Dequeue
            Dim ln As String() = line.Split(":")
            For n = 0 To ln.Length - 1
                ln(n) = ln(n).Trim
            Next

            Select Case ln(0).ToLower
                Case "type" : type = ln(1)
                Case "blueprint" : blueprint = buildBlueprint(ln(1))
                Case "size" : size = CInt(ln(1))
                Case "value" : value = CInt(ln(1))
                Case "crewrace" : crewRace = constants.getEnumFromString(ln(1), constants.raceArray)
                Case "crewmax" : crewMax = CInt(ln(1))
                Case "damage" : damage = CInt(ln(1))
                Case "damagetype" : damageType = constants.getEnumFromString(ln(1), constants.damageTypeArray)
                Case "defencetype" : defenceType = constants.getEnumFromString(ln(1), constants.defenceTypeArray)
                Case "speed" : speed = CInt(ln(1))
                Case "dodge" : dodge = CInt(ln(1))
                Case "resource" : resource = constants.getEnumFromString(ln(1), constants.resourceArray)
                Case "resourceproductiontimer" : resourceProductionTimer = CInt(ln(1))
                Case "resourceslot" : resourceSlot = constants.getEnumFromString(ln(1), constants.resourceArray)
                Case "resourceqtyperuse" : resourceQtyPerUse = CInt(ln(1))
                Case "crewablemin" : crewableMin = CInt(ln(1))
                Case "crewablemax" : crewableMax = CInt(ln(1))
            End Select
        End While

        Dim hc As hullComponent = Nothing
        Select Case type.ToLower
            Case "cargo" : hc = New hcCargo(targetName, size, resource, value, resourceSlot, resourceQtyPerUse)
            Case "crewquarters" : hc = New hcCrewQuarters(targetName, size, crewMax, crewRace, resourceSlot, resourceQtyPerUse)
            Case "defence" : hc = New hcDefence(targetName, size, defenceType, value, resourceSlot, resourceQtyPerUse)
            Case "engine" : hc = New hcEngine(targetName, size, speed, dodge, resourceSlot, resourceQtyPerUse)
            Case "jumpdrive" : hc = New hcJumpDrive(targetName, size, speed, resourceSlot, resourceQtyPerUse)
            Case "producer" : hc = New hcProducer(targetName, size, resource, resourceProductionTimer, resourceSlot, resourceQtyPerUse)
            Case "weapon" : hc = New hcWeapon(targetName, size, damage, damageType, resourceSlot, resourceQtyPerUse)
        End Select
        If hc Is Nothing = False Then hc.blueprint = blueprint
        buildCrewable(hc, crewableMin, crewableMax)
        Return hc
    End Function
    Private Shared Sub buildCrewable(ByRef hc As hullComponent, ByVal crewableMin As Integer, ByVal crewableMax As Integer)
        If hc Is Nothing Then Exit Sub
        If TypeOf hc Is ihcCrewable = False Then Exit Sub
        If crewableMin = 0 AndAlso crewableMax = 0 Then Exit Sub

        Dim i As ihcCrewable = CType(hc, ihcCrewable)
        i.crewable.SetProperties(crewableMin, crewableMax)
    End Sub
    Private Shared Function buildBlueprint(ByVal rawstr As String) As List(Of String)
        'sample format
        '1 container, 5 struts

        Dim total As New List(Of String)
        Dim ln As String() = rawstr.Split(",")
        For Each l In ln
            l = l.Trim
            total.Add(l)
        Next
        Return total
    End Function

    Public Overrides Function ToString() As String
        Return name
    End Function
    Friend Function consoleResourceDescription() As String
        If resourceSlot = Nothing Then Return Nothing
        Return "[" & resourceSlot.ToString & " " & resourceQtyRemaining & "%]"
    End Function
    Friend MustOverride Function consoleDescription() As String
    Friend Overridable ReadOnly Property alarms As List(Of String)
        Get
            Dim total As New List(Of String)
            If resourceSlot <> Nothing AndAlso resourceQtyRemaining = 0 Then total.Add("Load more " & resourceSlot.ToString & ".")
            Return total
        End Get
    End Property

    Protected _name As String
    Friend ReadOnly Property name As String
        Get
            Return _name
        End Get
    End Property
    Friend ship As ship
    Protected _size As Integer
    Friend ReadOnly Property size As Integer
        Get
            Return _size
        End Get
    End Property
    Private Property blueprint As New List(Of String)
    Friend MustOverride ReadOnly Property typeString As String

    Friend Overridable Sub tickTravel()
        'handle in subclass if necessary
    End Sub
    Friend Overridable Sub tickIdle()
        'handle in subclass if necessary
    End Sub

    Private resourceSlot As eResource
    Private resourceQtyRemaining As Integer
    Private resourceQtyPerUse As Integer
    Friend autoloadResource As Boolean = False
    Friend Sub setResourceSlot(ByVal aResourceSlot As eResource, ByVal aResourceQtyPerUse As Integer)
        resourceSlot = aResourceSlot
        resourceQtyPerUse = aResourceQtyPerUse
    End Sub
    Friend Overridable Function loadResource() As Boolean
        If resourceSlot = Nothing Then Return True
        If resourceQtyRemaining >= 100 Then Return False
        If ship.addResourceCheck(resourceSlot, -1) = False Then
            alert.Add("Load Failure", name & " was unable to load " & resourceSlot.ToString & " from the cargo hold.", 5)
            Return False
        End If

        ship.addResource(resourceSlot, -1)
        resourceQtyRemaining += 100
        alert.Add("Load", name & " loaded a pod of " & resourceSlot.ToString & " from the cargo hold.", 7)
        Return True
    End Function
    Protected Function useResource(Optional ByVal value As Integer = 1) As Boolean
        If resourceSlot = Nothing Then Return True
        Dim trueQty As Integer = resourceQtyPerUse * value
        If trueQty > resourceQtyRemaining Then
            If autoloadResource = True Then loadResource()
            alert.Add("Use Failure", name & " is out of " & resourceSlot.ToString & "!", 5)
            Return False
        End If

        resourceQtyRemaining -= trueQty
        Return True
    End Function
End Class
