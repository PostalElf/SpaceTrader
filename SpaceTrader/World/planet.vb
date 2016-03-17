Public Class planet
    Public Sub New(ByRef r As Random)
        For Each kvp In productPricesDefault
            productsPrices.Add(kvp.Key, kvp.Value)
        Next
        For Each kvp In servicePricesDefault
            servicesPrices.Add(kvp.Key, kvp.Value)
        Next
    End Sub
    Friend Shared Function build(ByRef star As star, ByVal planetNumber As Integer, ByRef r As Random) As planet
        Dim planet As New planet(r)
        With planet
            ._star = star
            .number = planetNumber

            Dim export As String = buildExport(r)
            Select Case export
                Case "Tourist"
                    'no export, imports x2
                    For n = 1 To 2
                        Dim import As eResource = constants.resourceArray(r.Next(constants.resourceArray.Length))
                        .productsImport.Add(import)
                    Next
                Case "Commercial"
                    'export random, import normal
                    Dim e As eResource = 0
                    While e <= 0 OrElse e > 100
                        e = constants.resourceArray(r.Next(constants.resourceArray.Length))
                    End While
                    .productsExport.Add(e)
                    .productsImport.Add(buildImport(e, r))
                Case Else
                    'prefix based on export, import normal
                    Dim e As eResource = constants.getEnumFromString(export, constants.resourceArray)
                    .productsExport.Add(e)
                    .productsImport.Add(buildImport(e, r))
            End Select
            .role = buildRole(export)
            .type = buildType(r)
            ._distanceToGate = buildDistanceToGate(.type, r)
            .habitation = buildHabitation(.type, r)

            .servicesAvailability = buildServiceAvailability(r, .role, .type)
            For n = 1 To 5
                .adjustProductPrices(r)
            Next
        End With
        Return planet
    End Function
    Private Shared planetExports As New List(Of String)
    Private Shared planetImports As New List(Of eResource)
    Private Shared planetRoles As New List(Of ePlanetType)
    Private Shared Function buildExport(ByRef r As Random) As String
        If planetExports.Count = 0 Then
            For Each res In constants.resourceArray
                If res < 100 Then planetExports.Add(res.ToString)
            Next
            planetExports.Add("Tourist")
            planetExports.Add("Commercial")
        End If

        Dim c As String = planetExports(r.Next(planetExports.Count))
        planetExports.Remove(c)
        Return c
    End Function
    Private Shared Function buildImport(ByVal export As eResource, ByRef r As Random) As eResource
        If planetImports.Count = 1 AndAlso planetImports.Contains(export) Then planetImports.Clear()
        If planetImports.Count = 0 Then
            For Each res As eResource In constants.resourceArray
                If res < 100 Then planetImports.Add(res)
            Next
        End If

        Dim c As eResource = export
        While c = export
            c = planetImports(r.Next(planetImports.Count))
        End While
        planetImports.Remove(c)
        Return c
    End Function
    Private Shared Function buildRole(ByVal export As String) As ePlanetRole
        Select Case export
            Case "Tourist" : Return ePlanetRole.Tourist
            Case "Commercial" : Return ePlanetRole.Commercial
        End Select

        Dim e As eResource = constants.getEnumFromString(export, constants.resourceArray)
        If e = Nothing Then
            MsgBox("buildRole error!")
            Return Nothing
        End If

        Select Case e
            Case eResource.Metals, eResource.Chemicals : Return ePlanetRole.Mining
            Case eResource.Ammunition, eResource.Missiles : Return ePlanetRole.Industrial
            Case eResource.Savants, eResource.Machines : Return ePlanetRole.Research
            Case eResource.Slaves, eResource.Azoth : Return ePlanetRole.Prison
            Case eResource.Food, eResource.Organics : Return ePlanetRole.Agarian
            Case eResource.Bandwidth, eResource.Media : Return ePlanetRole.Cultural
            Case Else
                MsgBox("buildRole error!")
                Return Nothing
        End Select
    End Function
    Private Shared Function buildType(ByRef r As Random) As ePlanetType
        If planetRoles.Count = 0 Then planetRoles.AddRange(constants.planetTypeArray)
        Dim c As ePlanetType = planetRoles(r.Next(planetRoles.Count))
        planetRoles.Remove(c)
        Return c
    End Function
    Private Shared Function buildHabitation(ByVal suffix As ePlanetType, ByRef r As Random) As String
        Dim x As Integer = r.Next(1, 101)

        Select Case suffix
            Case ePlanetType.Sprawl
                Select Case x
                    Case 1 To 50 : Return "Hivecities"
                    Case Else : Return "Cubecities"
                End Select
            Case ePlanetType.Wasteland
                Select Case x
                    Case 1 To 33 : Return "Sealed Arcologies"
                    Case 34 To 66 : Return "Underground Complex"
                    Case Else : Return "Orbital Ring"
                End Select
            Case ePlanetType.Eden
                Select Case x
                    Case 1 To 33 : Return "Hivecities"
                    Case 34 To 66 : Return "Undersea Domes"
                    Case Else : Return "Cubecities"
                End Select
            Case ePlanetType.Barren
                Select Case x
                    Case 1 To 33 : Return "Sealed Arcologies"
                    Case 34 To 66 : Return "Hivecities"
                    Case Else : Return "Underground Complex"
                End Select
            Case ePlanetType.Oceanic
                Select Case x
                    Case 1 To 50 : Return "Undersea Domes"
                    Case Else : Return "Floating Platforms"
                End Select
            Case ePlanetType.Desert
                Select Case x
                    Case 1 To 33 : Return "Cubecities"
                    Case 34 To 66 : Return "Sealed Arcologies"
                    Case Else : Return "Hivecities"
                End Select
            Case ePlanetType.Volcanic
                Select Case x
                    Case 1 To 50 : Return "Orbital Ring"
                    Case Else : Return "Floating Platforms"
                End Select
            Case ePlanetType.Gaseous
                Select Case x
                    Case 1 To 50 : Return "Floating Platforms"
                    Case Else : Return "Orbital Ring"
                End Select
            Case Else
                MsgBox("Bugcatch!")
                Return Nothing
        End Select
    End Function
    Private Shared Function buildDistanceToGate(ByVal planetType As ePlanetType, ByRef r As Random) As Integer
        Select Case planetType
            Case ePlanetType.Desert, ePlanetType.Volcanic, ePlanetType.Wasteland : Return r.Next(100, 300)
            Case ePlanetType.Eden, ePlanetType.Sprawl : Return r.Next(300, 400)
            Case ePlanetType.Gaseous, ePlanetType.Oceanic : Return r.Next(300, 600)
            Case ePlanetType.Barren : Return r.Next(600, 700)
            Case Else
                MsgBox("buildDistanceToGate() error: unrecognised planetType")
                Return Nothing
        End Select
    End Function
    Private Shared Function buildServiceAvailability(ByRef r As Random, ByVal role As ePlanetRole, ByVal type As ePlanetType) As Dictionary(Of Integer, Dictionary(Of eService, Integer))
        Dim total As New Dictionary(Of Integer, Dictionary(Of eService, Integer))
        For n = 1 To 4
            Dim penalty As Integer = constrain(20 * n, 0, 85)
            total.Add(n, New Dictionary(Of eService, Integer))
            total(n).Add(eService.Repair, 100)

            Dim storage As Integer = 100 - penalty
            Dim war As Integer = 100 - penalty
            Dim travel As Integer = 100 - penalty
            Dim production As Integer = 100 - penalty

            Select Case role
                Case ePlanetRole.Mining
                    war += 10
                    travel -= 10
                Case ePlanetRole.Industrial
                    storage -= 10
                    war += 10
                Case ePlanetRole.Research
                    storage -= 10
                    production += 20
                    war -= 10
                Case ePlanetRole.Prison
                    storage += 10
                    war -= 20
                    travel += 10
                Case ePlanetRole.Agarian
                    storage += 10
                    travel -= 10
                Case ePlanetRole.Cultural
                    war -= 5
                    production += 5
                Case ePlanetRole.Commercial
                Case ePlanetRole.Tourist
                    storage += 10
                    war -= 10
            End Select

            Select Case type
                Case ePlanetType.Sprawl
                    war += 5
                Case ePlanetType.Wasteland
                    storage += 5
                Case ePlanetType.Eden
                    war -= 5
                    production += 10
                Case ePlanetType.Barren
                    storage += 5
                Case ePlanetType.Oceanic
                    travel += 5
                Case ePlanetType.Desert
                    travel += 5
                Case ePlanetType.Volcanic
                    production += 5
                Case ePlanetType.Gaseous
                    travel += 5
            End Select

            With total(n)
                .Add(eService.Storage, constrain(storage, 15, 100))
                .Add(eService.War, constrain(war, 15, 100))
                .Add(eService.Travel, constrain(travel, 15, 100))
                .Add(eService.Production, constrain(production, 15, 100))
            End With
        Next
        Return total
    End Function

    Public Overrides Function ToString() As String
        Return name
    End Function
    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftLen As Integer = 10

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Role:", ftLen) & role.ToString)
        Console.WriteLine(indd & fakeTab("Type:", ftLen) & type.ToString)
        Console.WriteLine(indd & fakeTab("Habitat:", ftLen) & habitation)
        Console.WriteLine(indd & fakeTab("Orbital:", ftLen) & distanceToGate)
    End Sub
    Friend Sub consoleReportShop(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftLen As Integer = 13

        If productsImport.Count > 0 Then
            Dim inputList As New List(Of String)
            For Each p In productsImport
                inputList.Add(p.ToString)
            Next
            Console.Write(indd & "Imports:  ")
            Console.WriteLine(withCommas(inputList))
        End If
        If productsExport.Count > 0 Then
            Dim inputList As New List(Of String)
            For Each p In productsExport
                inputList.Add(p.ToString)
            Next
            Console.Write(indd & "Exports:  ")
            Console.WriteLine(withCommas(inputList))
        End If
        Console.WriteLine(indd & "Prices:")
        For Each product As eResource In constants.resourceArray
            Console.WriteLine(inddd & "└ " & fakeTab(product.ToString & ":", ftLen) & getProductPriceBuy(product) & "/" & getProductPriceSell(product))
        Next
    End Sub

    Private _star As star
    Friend ReadOnly Property star As star
        Get
            Return _star
        End Get
    End Property
    Friend ReadOnly Property name As String
        Get
            Return star.name & " " & romanNumeral(number)
        End Get
    End Property
    Private number As Integer
    Private _distanceToGate As Integer
    Friend ReadOnly Property distanceToGate As Integer
        Get
            Return _distanceToGate
        End Get
    End Property
    Friend Function getDistanceTo(ByRef destination As planet) As Integer
        If destination.star.Equals(star) = False Then Return -1

        Dim total As Integer = Math.Abs(distanceToGate - destination.distanceToGate)
        total = constrain(total, 1, Int32.MaxValue)
        Return total
    End Function

    Private role As ePlanetRole
    Private type As ePlanetType
    Private habitation As String
    Private productsPrices As New Dictionary(Of eResource, Integer)
    Friend Function getProductPriceSell(ByVal product As eResource) As Integer
        Dim total As Integer = productsPrices(product)
        If productsImport.Contains(product) Then total *= 1.5
        If productsExport.Contains(product) Then total /= 1.5
        Return total
    End Function
    Friend Function getProductPriceBuy(ByVal product As eResource) As Integer
        Dim total As Integer = getProductPriceSell(product) * 0.75
        Return total
    End Function
    Private Sub adjustProductPrices(Optional ByRef r As Random = Nothing)
        If r Is Nothing Then r = rng
        For Each res In constants.resourceArray
            Dim maxVariance As Integer = (productsPrices(res) * 0.1)
            Dim variance As Integer = lumpyRng(-maxVariance, maxVariance, r)
            productsPrices(res) += variance
            productsPrices(res) = constrain(productsPrices(res), productPricesRange(res))
        Next
        For Each s In constants.serviceArray
            Dim maxVariance As Integer = (servicesPrices(s) * 0.1)
            Dim variance As Integer = lumpyRng(-maxVariance, maxVariance, r)
            servicesPrices(s) += variance
            servicesPrices(s) = constrain(servicesPrices(s), servicePricesRange(s))
        Next
    End Sub
    Private productsImport As New List(Of eResource)
    Private productsExport As New List(Of eResource)
    Friend Sub addShipment(ByVal res As eResource, ByVal isImport As Boolean)
        Dim iList As List(Of eResource)
        If isImport = True Then iList = productsImport Else iList = productsExport

        If iList.Contains(res) Then Exit Sub
        iList.Add(res)
    End Sub
    Friend Sub removeShipment(ByVal res As eResource, ByVal isImport As Boolean)
        Dim iList As List(Of eResource)
        If isImport = True Then iList = productsImport Else iList = productsExport

        If iList.Contains(res) = False Then Exit Sub
        iList.Remove(res)
    End Sub

    Private servicesPrices As New Dictionary(Of eService, Integer)
    Private servicesAvailability As New Dictionary(Of Integer, Dictionary(Of eService, Integer))
    Friend Function getServicePrice(ByVal service As eService) As Integer
        Return servicesPrices(service)
    End Function
    Friend Function getServiceAvailability(ByVal service As eService, ByVal tier As Integer) As Integer
        If tier < 1 OrElse tier > 4 Then
            MsgBox("getServiceAvailability tier out of range")
            Return -1
        End If
        If service = Nothing Then Return -1

        Dim tierDict As Dictionary(Of eService, Integer) = servicesAvailability(tier)
        Return tierDict(service)
    End Function

    Private Const priceMin As Double = 0.5
    Private Const priceMax As Double = 1.5
    Private Shared productPricesDefault As Dictionary(Of eResource, Integer) = buildProductPricesDefault()
    Private Shared Function buildProductPricesDefault() As Dictionary(Of eResource, Integer)
        Dim total As New Dictionary(Of eResource, Integer)
        With total
            .Add(eResource.Metals, 100)
            .Add(eResource.Chemicals, 100)
            .Add(eResource.Ammunition, 50)
            .Add(eResource.Missiles, 75)
            .Add(eResource.Savants, 150)
            .Add(eResource.Machines, 100)
            .Add(eResource.Slaves, 100)
            .Add(eResource.Azoth, 200)
            .Add(eResource.Food, 50)
            .Add(eResource.Organics, 100)
            .Add(eResource.Bandwidth, 100)
            .Add(eResource.Media, 100)

            .Add(eResource.Drugs, 200)
            .Add(eResource.Lore, 220)
        End With
        Return total
    End Function
    Friend Shared productPricesRange As Dictionary(Of eResource, range) = buildProductPricesRange()
    Private Shared Function buildProductPricesRange()
        Dim total As New Dictionary(Of eResource, range)
        With total
            For Each kvp In productPricesDefault
                Dim min As Integer = kvp.Value * priceMin
                Dim max As Integer = kvp.Value * priceMax
                .Add(kvp.Key, New range(min, max))
            Next
        End With
        Return total
    End Function
    Private Shared servicePricesDefault As Dictionary(Of eService, Integer) = buildServicePricesDefault()
    Private Shared Function buildServicePricesDefault() As Dictionary(Of eService, Integer)
        Dim total As New Dictionary(Of eService, Integer)
        For Each es In constants.serviceArray
            total.Add(es, 100)
        Next

        'for repair, represents cost per armour
        'for all other services, represents percentage of base product cost
        total(eService.Repair) = 10
        Return total
    End Function
    Friend Shared servicePricesRange As Dictionary(Of eService, range) = buildServicePricesRange()
    Private Shared Function buildServicePricesRange() As Dictionary(Of eService, range)
        Dim total As New Dictionary(Of eService, range)
        With total
            For Each kvp In servicePricesDefault
                Dim min As Integer = kvp.Value * priceMin
                Dim max As Integer = kvp.Value * priceMax
                .Add(kvp.Key, New range(min, max))
            Next
        End With
        Return total
    End Function

    Friend Sub tick()
        adjustProductPrices()
    End Sub
End Class
