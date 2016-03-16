Module Module1
    Private starmap As starmap = starmap.build(3, 3, 513058105)
    Private player As New player
    Private ship As ship = ship.build(player, eShipType.Crusier)


    Sub Main()
        Console.SetWindowSize(120, 50)
        'starmap.consoleReport(0)
        'Console.ReadLine()

        player.addCredits(10000)
        For Each r In constants.resourceArray
            ship.addResource(r, 10)
        Next
        ship.addComponent(New hcWeapon("Chaingun", 5, 1, eDamageType.Ballistic))
        ship.addComponent(New hcCargo("Gaol", 5, eResource.Slaves, 30))
        ship.addComponent(New hcDefence("Shield Battery", 5, eDefenceType.Shields, 5))
        Dim aeroponics As New hcProducer("Aeroponics Bay", 5, eResource.Food, 1, 5, eResource.Organics, 5)
        aeroponics.crewable.SetProperties(1, 3)
        ship.addComponent(aeroponics)
        ship.addComponent(New hcEngine("Z-23 Moonbeam Engines", 5, 100, 1, eResource.Chemicals, 5))
        ship.addComponent(New hcJumpDrive("Whirlwind Jumpdrive", 5, 10, eResource.Azoth, 5))
        ship.addComponent(New hcCrewQuarters("Crew Cabin", 5, 5, eRace.Human, eResource.Food, 1))
        ship.addComponent(New hcCrewQuarters("Machinery Room", 5, 5, eRace.Uplifted, eResource.Machines, 1))
        For n = 1 To 3
            ship.addCrew(crew.build(eRace.Human))
        Next
        ship.allLoadResource()
        ship.allAssignCrewBest()

        ship.teleportTo(starmap.getPlanetRandom)
        ship.setTravelDestination(starmap.getPlanetRandom)

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
    Private Sub handleInput(ByVal cmd As String)
        Select Case cmd.ToLower
            Case ""
                ship.tick()
                starmap.tick()
            Case "starmap", "sm"
                Console.Clear()
                starmap.consoleReport(0)
                Console.ReadKey()
        End Select
    End Sub

End Module
