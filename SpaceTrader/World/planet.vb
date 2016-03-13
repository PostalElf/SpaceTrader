Public Class planet
    Public Sub New()
        For Each r In constants.resourceArray
            productsPrices.Add(r, 100)
        Next
    End Sub
    Friend Shared Function build(ByRef star As star, ByVal planetNumber As Integer, ByRef r As Random) As planet
        Dim planet As New planet
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
                    .productsPrices(buildImport(r)) += 50
                Case Else
                    'prefix based on export, import normal
                    Dim e As eResource = constants.getEnumFromString(export, constants.resourceArray)
                    .productsExport.Add(e)
                    .productsPrices(buildImport(r)) += 50
            End Select
            .role = buildRole(export)
            .type = buildType(r)
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
        Dim c As eResource = planetImports(rng.Next(planetImports.Count))
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
            Case eResource.Food, eResource.Water : Return ePlanetRole.Agarian
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

    Friend Sub consoleReport(ByVal indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)
        Dim inddd As String = vbSpace(indent + 2)
        Const ftLen As Integer = 10
        Const ftLen2 As Integer = 13

        Console.WriteLine(ind & name)
        Console.WriteLine(indd & fakeTab("Role:", ftLen) & role.ToString)
        Console.WriteLine(indd & fakeTab("Type:", ftLen) & type.ToString)
        Console.WriteLine(indd & fakeTab("Habitat:", ftLen) & habitation)
        Console.WriteLine(indd & "Exports:")
        For Each export In productsExport
            Console.WriteLine(inddd & "└ " & export.ToString)
        Next
        Console.WriteLine(indd & "Prices:")
        For Each product As eResource In constants.resourceArray
            Console.WriteLine(inddd & "└ " & fakeTab(product.ToString & ":", ftLen2) & getProductPrice(product))
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

    Private role As ePlanetRole
    Private type As ePlanetType
    Private habitation As String
    Private productsExport As New List(Of eResource)
    Private productsPrices As New Dictionary(Of eResource, Integer)
    Friend Function getProductPrice(ByVal product As eResource) As Integer
        Dim total As Integer = productsPrices(product)
        If productsExport.Contains(product) Then total *= 0.75
        Return total
    End Function
End Class
