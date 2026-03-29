namespace OBP200_RolePlayingGame;

public class RoomHandler
{
    public bool HandleRoom(Player player, Room room)
    {
        switch (room.Type)
        {
            case "treasure":
                return HandleTreasure(player);
            case "shop":
                return HandleShop(player);
            case "rest":
                return HandleRest(player);
            default:
                return true;
        }
    }

    private bool HandleTreasure(Player player)
    {
        Console.WriteLine("Du hittar en gammal kista...");
        var gold = new Random().Next(5, 16);
        Console.WriteLine($"Kistan innehåller {gold} guld!");
        player.AddGold(gold);
        return true;
    }

    private bool HandleShop(Player player)
    {
        Console.WriteLine("En vandrande köpman dyker upp!");
        Console.WriteLine("1) Köp hälsodryck (10 guld)");
        Console.WriteLine("2) Gå vidare");
        Console.Write("Val: ");

        var choice = (Console.ReadLine() ?? "").Trim();

        if (choice == "1")
        {
            if (player.SpendGold(10))
            {
                player.Inventory.AddItem("Potion");
                Console.WriteLine("Du köpte en hälsodryck!");
            }
            else
            {
                Console.WriteLine("Du har inte tillräckligt med guld.");
            }
        }

        return true;
    }

    private bool HandleRest(Player player)
    {
        Console.WriteLine("Du hittar en lägereld och vilar...");
        player.Heal(10);
        Console.WriteLine("Du återhämtar 10 HP!");
        return true;
    }
}