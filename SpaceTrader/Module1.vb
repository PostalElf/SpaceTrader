Module Module1

    Sub Main()
        Dim starmap As starmap = starmap.build(3, 3, 513058105)
        starmap.consoleReport(0)
        Console.WriteLine()

        Dim player As New player
        Dim ship As ship = ship.build(player, eShipType.Corvette)
        ship.consoleReport(0)
        Console.ReadLine()
    End Sub

End Module
