﻿Public Class planet
    Public Sub New(ByRef r As Random)
        productsPrices = New Dictionary(Of eResource, Integer)(productPricesDefault)
        saleHullComponentPrices = New Dictionary(Of eHcCategory, Integer)(saleHullComponentPricesDefault)

        For Each s In constants.saleHullComponentArray
            saleHullComponents.Add(s, Nothing)
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

            .saleHullComponentAvailability = buildServiceAvailability(.role, .type)
            For n = 1 To 5
                .adjustPrices(r)
            Next
            .adjustSaleHullComponents(r)
            .adjustSaleCraftComponents(r)
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
    Private Shared Function buildServiceAvailability(ByVal role As ePlanetRole, ByVal type As ePlanetType) As Dictionary(Of eHcCategory, Integer)
        Dim total As New Dictionary(Of eHcCategory, Integer)

        Dim storage As Integer = 100
        Dim war As Integer = 100
        Dim travel As Integer = 100
        Dim production As Integer = 100

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

        With total
            .Add(eHcCategory.Storage, constrain(storage, 15, 100))
            .Add(eHcCategory.War, constrain(war, 15, 100))
            .Add(eHcCategory.Travel, constrain(travel, 15, 100))
            .Add(eHcCategory.Production, constrain(production, 15, 100))
        End With
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
        Const ftLen As Integer = 13

        If productsImport.Count > 0 Then
            Dim inputList As New List(Of String)
            For Each p In productsImport
                inputList.Add(p.ToString)
            Next
            Console.Write(ind & "Imports:  ")
            Console.WriteLine(withCommas(inputList))
        End If
        If productsExport.Count > 0 Then
            Dim inputList As New List(Of String)
            For Each p In productsExport
                inputList.Add(p.ToString)
            Next
            Console.Write(ind & "Exports:  ")
            Console.WriteLine(withCommas(inputList))
        End If
        Console.WriteLine(ind & "Prices:")
        For Each product As eResource In constants.resourceArray
            Console.WriteLine(indd & "└ " & fakeTab(product.ToString & ":", ftLen) & getProductPriceBuy(product) & "/" & getProductPriceSell(product))
        Next

        Console.WriteLine(ind & "Shipyard Prices:")
        Const ftlen1 As Integer = 13
        For Each hcc As eHcCategory In constants.saleHullComponentArray
            Console.WriteLine(indd & "└ " & fakeTab(hcc.ToString & ":", ftlen1) & getSaleHullComponentPriceModifier(hcc) & "%")
        Next

        Console.WriteLine(ind & "Shipyard:")
        Dim ftlen2 As Integer = 0
        For Each saleable In saleHullComponentList
            If saleable.name.Length > ftlen2 Then ftlen2 = saleable.name.Length
        Next
        ftlen2 += 3
        For Each saleable In saleHullComponentList
            With saleable
                Dim cost As Integer = getSaleHullComponentPrice(saleable)
                Console.WriteLine(indd & "└ " & fakeTab(.name & ":", ftlen2) & "[" & .saleTimer.ToString("00") & "] " & "¥" & cost.ToString("N0"))
            End With
        Next
    End Sub
    Friend Sub consoleReportCraft(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)

        Dim ftlen As Integer = 0
        For Each c In craftComponents
            If c.name.Length > ftlen Then ftlen = c.name.Length
        Next
        ftlen += 3

        Console.WriteLine(ind & "Craft Components:")
        For Each c In craftComponents
            Console.WriteLine(indd & fakeTab(c.name & ":", ftlen) & "[" & c.saleTimer.ToString("00") & "] " & "¥" & getCraftComponentPriceSell(c.name).ToString("N0"))
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

    Private productsPrices As Dictionary(Of eResource, Integer)
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
    Const repairCostDefault As Integer = 10
    Private repairCost As Integer = repairCostDefault
    Private repairCostRange As New range(repairCostDefault * priceMin, repairCostDefault * priceMax)
    Friend Function getRepairCost() As Integer
        Return repairCost
    End Function
    Private saleHullComponents As New Dictionary(Of eHcCategory, saleHullcomponent)
    Private saleHullComponentAvailability As New Dictionary(Of eHcCategory, Integer)
    Private saleHullComponentPrices As New Dictionary(Of eHcCategory, Integer)
    Friend Function getSaleHullComponentPriceModifier(ByVal service As eHcCategory) As Integer
        Return saleHullComponentPrices(service)
    End Function
    Friend Function getSaleHullComponentPrice(ByRef s As saleHullcomponent) As Integer
        Dim cost As Integer = s.saleCost / getSaleHullComponentPriceModifier(s.service) * 100
        Return cost
    End Function
    Friend Function saleHullComponentList() As List(Of saleable)
        Dim total As New List(Of saleable)
        For Each kvp In saleHullComponents
            If kvp.Value Is Nothing = False Then total.Add(kvp.Value)
        Next
        Return total
    End Function
    Private craftComponents As New List(Of saleCraftComponent)
    Private craftComponentsPriceModifier As Integer = 100
    Friend Function getCraftComponentPriceSell(ByVal c As String) As Integer
        Dim total As Integer = saleCraftComponent.getDefaultPrice(c) * craftComponentsPriceModifier / 100
        Return total
    End Function
    Friend Function getCraftComponentPriceBuy(ByVal c As String) As Integer
        Dim total As Integer = getCraftComponentPriceBuy(c) * 0.75
        Return total
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
    Friend Shared Function productPricesRange(ByVal p As eResource) As range
        Return New range(productPricesDefault(p) * priceMin, productPricesDefault(p) * priceMax)
    End Function
    Private Shared saleHullComponentPricesDefault As Dictionary(Of eHcCategory, Integer) = buildSaleHullComponentPricesDefault()
    Private Shared Function buildSaleHullComponentPricesDefault() As Dictionary(Of eHcCategory, Integer)
        'represents percentage of base product cost
        Dim total As New Dictionary(Of eHcCategory, Integer)
        For Each es In constants.saleHullComponentArray
            total.Add(es, 100)
        Next
        Return total
    End Function
    Friend Shared Function saleHullComponentPricesRange(ByVal s As eHcCategory) As range
        Return New range(saleHullComponentPricesDefault(s) * priceMin, saleHullComponentPricesDefault(s) * priceMax)
    End Function

    Friend Sub tick()
        adjustPrices()
        adjustSaleHullComponents()
        adjustSaleCraftComponents()
    End Sub
    Private Sub adjustPrices(Optional ByRef r As Random = Nothing)
        If r Is Nothing Then r = rng
        For Each res In constants.resourceArray
            Dim maxVariance As Integer = productsPrices(res) * 0.1
            Dim variance As Integer = lumpyRng(-maxVariance, maxVariance, r)
            productsPrices(res) += variance
            productsPrices(res) = constrain(productsPrices(res), productPricesRange(res))
        Next

        For Each s In constants.saleHullComponentArray
            Dim maxVariance As Integer = saleHullComponentPrices(s) * 0.1
            Dim variance As Integer = lumpyRng(-maxVariance, maxVariance, r)
            saleHullComponentPrices(s) += variance
            saleHullComponentPrices(s) = constrain(saleHullComponentPrices(s), saleHullComponentPricesRange(s))
        Next

        Dim repairVariance As Integer
        If repairCost = repairCostRange.min Then : repairVariance = 1
        ElseIf repairCost = repairCostRange.max Then : repairCost = -1
        Else : If coinFlip(r) = True Then repairVariance = 1 Else repairVariance = -1
        End If
        repairCost += repairVariance
        repairCost = constrain(repairCost, repairCostRange)

        Dim craftComponentVarianceMax As Integer = craftComponentsPriceModifier * 0.1
        Dim craftComponentVariance As Integer = lumpyRng(-craftComponentVarianceMax, craftComponentVarianceMax, r)
        Dim craftComponentVarianceRange As New range(50, 150)
        craftComponentsPriceModifier += craftComponentVariance
        craftComponentsPriceModifier = constrain(craftComponentsPriceModifier, craftComponentVarianceRange)
    End Sub
    Private Sub adjustSaleHullComponents(Optional ByRef r As Random = Nothing)
        If r Is Nothing Then r = rng

        For Each s As eHcCategory In constants.saleHullComponentArray
            Dim current As saleable = saleHullComponents(s)

            'tick if current is present
            If current Is Nothing = False Then current.tickSale()

            'roll spawnchance if current is empty
            'this is outside of tick check because there's a chance that the current may have expired,
            'in which case a new saleable is now added
            Dim spawnChance As Integer = saleHullComponentAvailability(s)
            If current Is Nothing Then
                If percentRoll(spawnChance, r) = True Then
                    Dim tier As Integer
                    Select Case r.Next(1, 11)
                        Case 1 To 4 : tier = 1
                        Case 5 To 7 : tier = 2
                        Case 8 To 9 : tier = 3
                        Case 10 : tier = 4
                    End Select

                    Dim shc As saleHullcomponent = saleHullcomponent.buildRandom(tier, s, r)
                    If shc Is Nothing = False Then
                        shc.parentServices = saleHullComponents
                        saleHullComponents(s) = shc
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub adjustSaleCraftComponents(Optional ByRef r As Random = Nothing)
        If r Is Nothing Then r = rng

        For n = craftComponents.Count - 1 To 0 Step -1
            Dim c As saleCraftComponent = craftComponents(n)
            c.tickSale()
        Next

        While craftComponents.Count < 5
            Dim tier As Integer
            Select Case r.Next(1, 11)
                Case 1 To 4 : tier = 1
                Case 5 To 7 : tier = 2
                Case 8 To 9 : tier = 3
                Case 10 : tier = 4
            End Select

            Dim c As saleCraftComponent = saleCraftComponent.buildRandom(tier, r)
            c.parentSaleList = craftComponents
            craftComponents.Add(c)
        End While
    End Sub
End Class
