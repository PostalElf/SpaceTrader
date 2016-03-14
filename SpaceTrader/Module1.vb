Module Module1

    Sub Main()
        Console.SetWindowSize(120, 50)
        Dim starmap As starmap = starmap.build(3, 3, 513058105)
        'starmap.consoleReport(0)
        'Console.WriteLine()

        Dim player As New player
        player.addCredits(10000)
        Dim ship As ship = ship.build(player, eShipType.Crusier)
        For Each r In constants.resourceArray
            ship.addResource(r, 10)
        Next
        ship.addComponent(New hcWeapon("Chaingun", 5, 1, eDamageType.Ballistic))
        ship.addComponent(New hcCargo("Gaol", 5, eResource.Slaves, 30))
        ship.addComponent(New hcDefence("Shield Battery", 5, eDefenceType.Shields, 5))
        ship.addComponent(New hcProducer("Aeroponics Bay", 5, eResource.Food, 1, 5, eResource.Organics, 5))
        ship.addComponent(New hcEngine("Z-23 Moonbeam Engines", 5, 5, 1, eResource.Chemicals, 5))
        ship.addComponent(New hcJumpDrive("Whirlwind Jumpdrive", 5, 50, eResource.Azoth, 5))
        ship.addComponent(New hcCrewQuarters("Crew Cabin", 5, 5, eResource.Food, 1))
        ship.loadResourceAll()

        While True
            Console.Clear()
            ship.consoleReport(0)
            Console.ReadLine()
            ship.tickTravel()
        End While
    End Sub

End Module
