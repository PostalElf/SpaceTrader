﻿Module Module1
    Private starmap As starmap
    Private player As New player
    Private ship As ship


    Sub Main()
        starmap = starmap.build(7, 3, 513058105)
        ship = ship.build(player, eShipType.Crusier)

        Console.SetWindowSize(120, 50)
        'starmap.consoleReport(0)
        'Console.ReadLine()

        player.addCredits(10000)
        For Each r In constants.resourceArray
            ship.addResource(r, 10)
        Next
        ship.addComponent(hullComponent.build("Food Megastorage"))
        ship.addComponent(hullComponent.build("Crew Cabin"))
        ship.addComponent(hullComponent.build("Uplifted Maintenance Bay"))
        ship.addComponent(hullComponent.build("Metal Containers"))
        For n = 1 To 3
            ship.addCrew(crew.build(eRace.Human))
        Next
        ship.addCrew(crew.build(eRace.Uplifted))
        ship.allLoadResource()
        ship.allAssignCrewBest()
        ship.teleportTo(starmap.stars(0).planets(0))

        While True
            Console.Clear()
            ship.consoleReport(0)
            Console.WriteLine(vbSpace(1) & "Alarms:")
            ship.consoleReportAlarms(2)
            Console.WriteLine()
            alert.allConsoleReport(0, 5)

            Console.Write("> ")
            handleInput(Console.ReadLine())
        End While
    End Sub
    Private Sub handleInput(ByVal rawstr As String)
        Dim cmd As String() = rawstr.Split(" ")
        Select Case cmd(0).ToLower
            Case ""
                ship.tick()
                starmap.tick()
            Case "cmd", "?", "help"
                cmdHelp()
            Case "exit"
                End
            Case "starmap", "sm"
                Console.Clear()
                starmap.consoleReport(0)
                Console.ReadKey()
            Case "travel", "t"
                cmdTravel(cmd)


            Case "adddamage"
                Dim damage As Integer = CInt(cmd(1))
                ship.addDamage(damage, eDamageType.Ballistic)
            Case "addimport"
                If ship.planet Is Nothing Then Exit Sub
                ship.planet.addShipment(getResourceFromStr(cmd(1)), True)
            Case "addexport"
                If ship.planet Is Nothing Then Exit Sub
                ship.planet.addShipment(getResourceFromStr(cmd(1)), False)
            Case "loadresources", "loadresource", "lr", "lrs"
                cmdLoadResource(cmd)


            Case "shop", "sh"
                If ship.planet Is Nothing Then Exit Sub
                Console.Clear()
                ship.planet.consoleReport(0)
                ship.planet.consoleReportShop(1)
                Console.ReadKey()
            Case "shipyardshop", "sysh"
                If ship.planet Is Nothing Then Exit Sub
                Console.Clear()
                ship.planet.consoleReportShipyard(0)
                Console.ReadKey()
            Case "buy"
                cmdBuySell(cmd, True)
            Case "sell"
                cmdBuySell(cmd, False)
            Case "shipyardbuy", "sybuy"
                cmdBuyShipyard(cmd)
            Case "shipyardsell", "sysell"
                cmdSellShipyard(cmd)
            Case "examine", "ex"
                cmdExamine(cmd)
            Case "shipyardexamine", "syex"
                cmdExamineShipyard(cmd)
            Case "repair", "rep"
                cmdRepair()
        End Select
    End Sub
    Private Sub cmdHelp()
        Dim ind As String = vbSpace(1)
        Const ftlen As Integer = 15

        Console.Clear()
        Console.WriteLine("Commands:")
        Console.WriteLine(ind & fakeTab("starmap/sm:", ftlen) & "reveals starmap")
        Console.WriteLine(ind & fakeTab("travel/t:", ftlen) & "set travel destination")
        Console.WriteLine(ind & fakeTab("travel c:", ftlen) & "cancel travel plans")
        Console.WriteLine(ind & fakeTab("shop:", ftlen) & "examines planet stores")
        Console.WriteLine(ind & fakeTab("examine", ftlen) & "examines specific good on the planet")
        Console.WriteLine(ind & fakeTab("buy <good>", ftlen) & "buy goods")
        Console.WriteLine(ind & fakeTab("sell <good>", ftlen) & "sell goods")
        Console.WriteLine()
        Console.WriteLine("Goods:")
        Console.WriteLine(ind & fakeTab("1/me:", ftlen) & "metals")
        Console.WriteLine(ind & fakeTab("2/ch:", ftlen) & "chemicals")
        Console.WriteLine(ind & fakeTab("3/am:", ftlen) & "ammunition")
        Console.WriteLine(ind & fakeTab("4/mi:", ftlen) & "missiles")
        Console.WriteLine(ind & fakeTab("5/sa:", ftlen) & "savants")
        Console.WriteLine(ind & fakeTab("6/ma:", ftlen) & "machines")
        Console.WriteLine(ind & fakeTab("7/sl:", ftlen) & "slaves")
        Console.WriteLine(ind & fakeTab("8/az:", ftlen) & "azoth")
        Console.WriteLine(ind & fakeTab("9/fd:", ftlen) & "food")
        Console.WriteLine(ind & fakeTab("10/or:", ftlen) & "organics")
        Console.WriteLine(ind & fakeTab("11/bd:", ftlen) & "bandwidth")
        Console.WriteLine(ind & fakeTab("12/md:", ftlen) & "media")
        Console.ReadKey()
    End Sub
    Private Sub cmdBuySell(ByRef cmd As String(), ByVal isBuying As Boolean)
        Dim ind As String = vbSpace(1)
        Dim r As eResource = getResourceFromStr(cmd(1))
        Dim descriptor As String
        If isBuying = True Then descriptor = "Buy" Else descriptor = "Sell"

        If ship.planet Is Nothing Then Exit Sub
        If r = Nothing Then Exit Sub

        Console.Write(vbCrLf & ind & descriptor & " how many pods of " & r.ToString & "? ")
        Dim qtyInput As String = Console.ReadLine()
        If IsNumeric(qtyInput) = False Then Exit Sub

        Dim qty As Integer = CInt(qtyInput)
        Dim price As Integer
        If isBuying = True Then price = ship.planet.getProductPriceSell(r) Else price = ship.planet.getProductPriceBuy(r)
        Dim totalPrice As Integer = qty * price

        If menu.confirmChoice(1, descriptor & " " & qty & " " & r.ToString & " for ¥" & totalPrice & "? ") = False Then Exit Sub
        If isBuying = True Then
            If player.addCreditsCheck(-totalPrice) = False Then
                Console.WriteLine(vbCrLf & ind & "Insufficient credits!")
                Console.ReadKey()
                Exit Sub
            End If
            If ship.addResourceCheck(r, qty) = False Then
                Console.WriteLine(vbCrLf & ind & "Insufficient cargo space!")
                Console.ReadKey()
                Exit Sub
            End If

            player.addCredits(-totalPrice)
            ship.addResource(r, qty)
        Else
            If ship.addResourceCheck(r, -qty) = False Then
                Console.WriteLine(vbCrLf & ind & "Insufficient cargo!")
                Console.ReadKey()
                Exit Sub
            End If

            ship.addResource(r, -qty)
            player.addCredits(totalPrice)
        End If
    End Sub
    Private Sub cmdBuyShipyard(ByRef cmd As String())
        If ship.planet Is Nothing Then Exit Sub
        Dim i As Integer = CInt(cmd(1))
        If i < 1 OrElse i > ship.planet.saleHullComponentList.Count Then Exit Sub
        Dim s As saleable = ship.planet.saleHullComponentList(i - 1)
        If s Is Nothing Then Exit Sub

        Dim ind As String = vbSpace(1)
        If menu.confirmChoice(0, vbCrLf & ind & "Buy " & s.name & " for ¥" & ship.planet.getSaleHullComponentPrice(s).ToString("N0") & "? ") = False Then Exit Sub

        Dim cost As Integer = ship.planet.getSaleHullComponentPrice(s)
        If player.addCreditsCheck(-cost) = False Then
            Console.WriteLine("Insufficient credits!")
            Console.ReadKey()
            Exit Sub
        End If
        Dim hc As hullComponent = s.unpack
        If ship.addComponentCheck(hc) = False Then
            Console.WriteLine("Insufficient hull space on ship!")
            Console.ReadKey()
            Exit Sub
        End If

        player.addCredits(-cost)
        ship.addComponent(hc)
        ship.planet.saleHullComponentList(i - 1).expire()
    End Sub
    Private Sub cmdSellShipyard(ByRef cmd As String())
        If ship.planet Is Nothing Then Exit Sub

        Console.WriteLine()
        Dim hc As hullComponent = menu.getListChoice(ship.hullComponentsList, 0, "Disassemble which hull component?")
        If hc Is Nothing Then Exit Sub
    End Sub
    Private Sub cmdRepair()
        If ship.planet Is Nothing Then Exit Sub

        Dim defences As Integer() = ship.getDefences("armour")
        Dim armour As Integer = defences(0)
        Dim armourMax As Integer = defences(1)
        Dim empty As Integer = armourMax - armour
        Dim price As Integer = ship.planet.getRepairCost()
        Dim totalPrice As Integer = empty * price

        Console.WriteLine()
        Console.WriteLine(fakeTab("Repair:", 9) & "¥" & price.ToString("N0"))
        Console.WriteLine(fakeTab("Armour:", 9) & armour & "/" & armourMax)
        If empty <= 0 Then
            Console.WriteLine("The ship is in perfect condition.")
            Console.ReadKey()
            Exit Sub
        End If

        If menu.confirmChoice(0, "Pay ¥" & totalPrice.ToString("N0") & " for full repairs? ") = True Then
            If player.addCreditsCheck(-totalPrice) = False Then
                Console.WriteLine(vbCrLf & "Insufficient funds.")
                Console.ReadKey()
                Exit Sub
            End If
            player.addCredits(-totalPrice)
            ship.repair(empty)
        Else

        End If
    End Sub
    Private Sub cmdExamine(ByRef cmd As String())
        Dim ind As String = vbSpace(1)
        Dim r As eResource = getResourceFromStr(cmd(1))
        Const ftlen As Integer = 8

        If ship.planet Is Nothing Then Exit Sub
        If r = Nothing Then Exit Sub

        Console.WriteLine()
        Console.WriteLine(r.ToString)
        With ship.planet
            Console.WriteLine(ind & fakeTab("Buy:", ftlen) & "¥" & .getProductPriceBuy(r))
            Console.WriteLine(ind & fakeTab("Sell:", ftlen) & "¥" & .getProductPriceSell(r))
            Console.WriteLine(ind & fakeTab("Range:", ftlen) & "¥" & planet.productPricesRange(r).min & " - ¥" & planet.productPricesRange(r).max)
        End With
        Console.ReadKey()
    End Sub
    Private Sub cmdExamineShipyard(ByRef cmd As String())
        If ship.planet Is Nothing Then Exit Sub
        If cmd.Length < 2 Then Exit Sub
        Dim index As Integer = CInt(cmd(1))
        If index < 1 OrElse index > ship.planet.saleHullComponentList.Count Then Exit Sub
        Dim s As saleable = ship.planet.saleHullComponentList(index - 1)
        If s Is Nothing Then Exit Sub

        Dim ind As String = vbSpace(1)
        Dim cost As Integer = ship.planet.getSaleHullComponentPrice(s)
        Const ftlen As Integer = 14

        Console.WriteLine()
        Console.WriteLine(s.name)
        Console.WriteLine(ind & fakeTab("Sale Period:", ftlen) & s.saleTimer & " ticks remaining")
        Console.WriteLine(ind & fakeTab("Sale Cost:", ftlen) & "¥" & cost.ToString("N0"))
        Console.WriteLine(ind & fakeTab("Effect:", ftlen) & s.consoleDescription)
        Console.ReadKey()
    End Sub
    Private Sub cmdTravel(ByRef cmd As String())
        Console.WriteLine()
        If cmd.Length > 1 AndAlso (cmd(1) = "c" OrElse cmd(1) = "cancel") Then
            ship.setTravelDestination(Nothing)
            Console.WriteLine("Travel plans cancelled.")
            Console.ReadKey()
        ElseIf cmd.Length = 3 Then
            Dim destination As planet = starmap.getPlanet(cmd(1), CInt(cmd(2)))
            If destination Is Nothing Then
                Console.WriteLine("Invalid travel destination.")
                Console.ReadKey()
                Exit Sub
            End If

            If menu.confirmChoice(0, "Travel to " & destination.name & "? ") = False Then Exit Sub
            ship.setTravelDestination(destination)
        Else
            Dim star As star = menu.getListChoice(starmap.stars, 1, "Select a star:")
            If star Is Nothing Then Exit Sub
            Console.WriteLine()
            Dim destination As planet = menu.getListChoice(star.planets, 1, "Select a planet:")
            If destination Is Nothing Then Exit Sub
            Console.WriteLine()

            If menu.confirmChoice(0, "Travel to " & destination.name & "? ") = False Then Exit Sub
            ship.setTravelDestination(destination)
        End If
    End Sub
    Private Sub cmdLoadResource(ByVal cmd As String())
        If cmd(0) = "lrs" OrElse cmd(0) = "loadresources" Then
            ship.allLoadResource()
            Exit Sub
        End If

        Console.WriteLine()
        Dim choice As hullComponent = menu.getListChoice(ship.hullComponentsList, 1, "Select a hull component:")
        If choice Is Nothing Then Exit Sub
        If choice.loadResource() = False Then
            Console.WriteLine("Load failure.")
            Console.ReadKey()
        End If
    End Sub


    Private Function getResourceFromStr(ByVal rawstr As String) As eResource
        Dim r As eResource = constants.getEnumFromString(rawstr, constants.resourceArray)
        If r <> Nothing Then Return r

        Select Case rawstr.ToLower
            Case "1", "me" : Return eResource.Metals
            Case "2", "ch" : Return eResource.Chemicals
            Case "3", "am" : Return eResource.Ammunition
            Case "4", "mi" : Return eResource.Missiles
            Case "5", "sa" : Return eResource.Savants
            Case "6", "ma" : Return eResource.Machines
            Case "7", "sl" : Return eResource.Slaves
            Case "8", "az" : Return eResource.Azoth
            Case "9", "fd" : Return eResource.Food
            Case "10", "or" : Return eResource.Organics
            Case "11", "bd" : Return eResource.Bandwidth
            Case "12", "md" : Return eResource.Media
            Case "13", "dg" : Return eResource.Drugs
            Case "14", "le" : Return eResource.Lore
            Case Else : Return Nothing
        End Select
    End Function
End Module
