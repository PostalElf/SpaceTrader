Module Module1

    Sub Main()
        Dim starmap As starmap = starmap.build(3, 3, 513058105)
        starmap.consoleReport(0)
        Console.WriteLine()

        Dim player As New player
        player.addCredits(10000)
        Dim ship As ship = ship.build(player, eShipType.Corvette)
        ship.addComponent(New hcWeapon("Chaingun", 5, 1, eDamageType.Ballistic))
        ship.addComponent(New hcCargo("Gaol", 5, eResource.Slaves, 30))
        ship.addComponent(New hcDefence("Shield Battery", 5, eDefenceType.Shields, 5))
        ship.consoleReport(0)
        Console.ReadLine()
    End Sub

End Module
