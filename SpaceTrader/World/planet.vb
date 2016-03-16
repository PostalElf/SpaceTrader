Public Class planet
    Public Sub New(ByRef r As Random)
        For Each kvp In productPricesDefault
            productsPrices.Add(kvp.Key, kvp.Value)
        Next
        For n = 1 To 5
            adjustProductPrices(r)
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
                        .productsPrices(import) += 50
                    Next
                Case "Commercial"
                    'export random, import normal
                    Dim e As eResource = constants.resourceArray(r.Next(constants.resourceArray.Length))
                    .productsExport.Add(e)
                    .productsPrices(buildImport(r)) *= 1.3
                Case Else
                    'prefix based on export, import normal
                    Dim e As eResource = constants.getEnumFromString(export, constants.resourceArray)
                    .productsExport.Add(e)
                    .productsPrices(buildImport(r)) *= 1.3
            End Select
            .role = buildRole(export)
            .type = buildType(r)
            ._distanceToGate = buildDistanceToGate(.type, r)
            .habitation = buildHabitation(.type, r)
        End With
        Return planet
    End Function
    Private Shared planetExports As New List(Of String)
    Private Shared planetImports As New List(Of eResource)
    Private Shared planetSuffixes As New List(Of ePlanetType)
    Private Shared Function buildExport(ByRef r As Random) As String
        If planetExports.Count = 0 Then
            For Each res In constants.resourceArray
                planetExports.Add(res.ToString)
            Next
            planetExports.Add("Tourist")
            planetExports.Add("Commercial")
        End If

        Dim c As String = planetExports(r.Next(planetExports.Count))
        planetExports.Remove(c)
        Return c
    End Function
    Private Shared Function buildImport(ByRef r As Random) As eResource
        If planetImports.Count = 0 Then planetImports.AddRange(constants.resourceArray)
        Dim c As eResource = planetImports(r.Next(planetImports.Count))
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
        If planetSuffixes.Count = 0 Then planetSuffixes.AddRange(constants.planetSuffixArray)
        Dim c As ePlanetType = planetSuffixes(r.Next(planetSuffixes.Count))
        planetSuffixes.Remove(c)
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
    Private Shared productPricesDefault As Dictionary(Of eResource, Integer) = buildProductPricesDefault()
    Private Shared Function buildProductPricesDefault() As Dictionary(Of eResource, Integer)
        Dim total As New Dictionary(Of eResource, Integer)
        With total
            .Add(eResource.Metals, 100)
            .Add(eResource.Chemicals, 100)
            .Add(eResource.Ammunition, 50)
            .Add(eResource.Missiles, 70)
            .Add(eResource.Savants, 150)
            .Add(eResource.Machines, 100)
            .Add(eResource.Slaves, 100)
            .Add(eResource.Azoth, 200)
            .Add(eResource.Food, 50)
            .Add(eResource.Organics, 50)
            .Add(eResource.Bandwidth, 120)
            .Add(eResource.Media, 100)
        End With
        Return total
    End Function
    Friend Shared productPricesRange As Dictionary(Of eResource, range) = buildProductPricesRange()
    Private Shared Function buildProductPricesRange()
        Dim total As New Dictionary(Of eResource, range)
        With total
            For Each kvp In productPricesDefault
                Dim min As Integer = kvp.Value * 0.5
                Dim max As Integer = kvp.Value * 1.5
                .Add(kvp.Key, New range(min, max))
            Next
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
        Dim inddd As String = vbSpace(indent + 2)
        Const ftLen As Integer = 13

        Console.WriteLine(indd & "Exports:")
        For Each export In productsExport
            Console.WriteLine(inddd & "└ " & export.ToString)
        Next
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

        Return Math.Abs(distanceToGate - destination.distanceToGate)
    End Function

    Private role As ePlanetRole
    Private type As ePlanetType
    Private habitation As String
    Private productsExport As New List(Of eResource)
    Private productsPrices As New Dictionary(Of eResource, Integer)
    Friend Function getProductPriceSell(ByVal product As eResource) As Integer
        Dim total As Integer = productsPrices(product)
        If productsExport.Contains(product) Then total /= 1.5
        Return total
    End Function
    Friend Function getProductPriceBuy(ByVal product As eResource) As Integer
        Return getProductPriceSell(product) * 0.75
    End Function
    Private Sub adjustProductPrices(Optional ByRef r As Random = Nothing)
        If r Is Nothing Then r = rng
        For Each res In constants.resourceArray
            Dim maxVariance As Integer = (productsPrices(res) * 0.1)
            Dim variance As Integer = lumpyRng(-maxVariance, maxVariance, r)
            productsPrices(res) += variance
            productsPrices(res) = constrain(productsPrices(res), productPricesRange(res))
        Next
    End Sub

    Friend Sub tick()
        adjustProductPrices()
    End Sub
End Class
