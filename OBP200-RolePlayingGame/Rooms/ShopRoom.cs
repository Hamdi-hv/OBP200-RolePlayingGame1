namespace OBP200_RolePlayingGame.Rooms;

using Characters;

/// <summary>
/// A merchant shop where the player can buy and sell items.
/// </summary>
public class ShopRoom : Room
{
    public ShopRoom(string label) : base(label) { }

    public override bool Enter(Player player)
    {
        Console.WriteLine("En vandrande köpman erbjuder sina varor:");

        while (true)
        {
            Console.WriteLine($"Guld: {player.Gold} | Drycker: {player.Potions}");
            Console.WriteLine("1) Köp dryck      (10 guld)");
            Console.WriteLine("2) Köp vapen      (+2 ATK, 25 guld)");
            Console.WriteLine("3) Köp rustning   (+2 DEF, 25 guld)");
            Console.WriteLine("4) Sälj Minor Gem (+5 guld/st)");
            Console.WriteLine("5) Lämna butiken");
            Console.Write("Val: ");

            var choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1": BuyPotion(player);  break;
                case "2": BuyWeapon(player);  break;
                case "3": BuyArmor(player);   break;
                case "4": SellGems(player);   break;
                case "5":
                    Console.WriteLine("Du säger adjö till köpmannen.");
                    return true;
                default:
                    Console.WriteLine("Köpmannen förstår inte ditt val.");
                    break;
            }
        }
    }

    private static void BuyPotion(Player player)
    {
        if (player.SpendGold(10)) { player.AddPotion(); Console.WriteLine("Du köper en dryck."); }
        else Console.WriteLine("Du har inte råd.");
    }

    private static void BuyWeapon(Player player)
    {
        if (player.SpendGold(25)) { player.UpgradeAttack(2); Console.WriteLine("Du köper ett bättre vapen."); }
        else Console.WriteLine("Du har inte råd.");
    }

    private static void BuyArmor(Player player)
    {
        if (player.SpendGold(25)) { player.UpgradeDefense(2); Console.WriteLine("Du köper bättre rustning."); }
        else Console.WriteLine("Du har inte råd.");
    }

    private static void SellGems(Player player)
    {
        if (player.RemoveItems("Minor Gem", out int count))
        {
            player.AddGold(count * 5);
            Console.WriteLine($"Du säljer {count} st Minor Gem för {count * 5} guld.");
        }
        else
        {
            Console.WriteLine("Inga 'Minor Gem' i väskan.");
        }
    }
}

