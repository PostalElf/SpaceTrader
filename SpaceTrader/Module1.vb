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
        Dim aeroponics As New hcProducer("Aeroponics Bay", 5, eResource.Food, 1, 5)
        aeroponics.setResourceSlot(eResource.Organics, 5)
        ship.addComponent(aeroponics)
        aeroponics.loadResource()
        Dim moonbeam As New hcEngine("Z-23 Moonbeam Engines", 5, 5, 1)
        moonbeam.setResourceSlot(eResource.Azoth, 5)
        ship.addComponent(moonbeam)
        moonbeam.loadResource()
        Dim whirlwind As New hcJumpDrive("Whirlwind Jumpdrive", 5, 50)
        whirlwind.setResourceSlot(eResource.Chemicals, 5)
        ship.addComponent(whirlwind)
        whirlwind.loadResource()

        While True
            Console.Clear()
            ship.consoleReport(0)
            Console.ReadLine()
            ship.tickTravel()
        End While
    End Sub

End Module
