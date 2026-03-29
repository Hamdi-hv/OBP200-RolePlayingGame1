using System.Text;

namespace OBP200_RolePlayingGame;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("=== Text-RPG ===");
            Console.WriteLine("1. Nytt spel");
            Console.WriteLine("2. Avsluta");
            Console.Write("Val: ");

            var choice = (Console.ReadLine() ?? "").Trim();

            if (choice == "1")
            {
                var player = CreatePlayer();
                RunGame(player);
            }
            else if (choice == "2")
            {
                Console.WriteLine("Avslutar...");
                return;
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }
        }
    }

    static Player CreatePlayer()
    {
        Console.Write("Ange namn: ");
        var name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name))
            name = "Namnlös";

        Console.WriteLine("Välj klass: 1) Warrior  2) Mage  3) Rogue");
        Console.Write("Val: ");
        var k = (Console.ReadLine() ?? "").Trim();

        string cls;
        int maxHp, atk, def, potions, gold;

        switch (k)
        {
            case "2":
                cls = "Mage";
                maxHp = 28; atk = 10; def = 2; potions = 2; gold = 15;
                break;
            case "3":
                cls = "Rogue";
                maxHp = 32; atk = 8; def = 3; potions = 3; gold = 20;
                break;
            default:
                cls = "Warrior";
                maxHp = 40; atk = 7; def = 5; potions = 2; gold = 15;
                break;
        }

        return new Player(name, cls, maxHp, atk, def, gold, potions);
    }

    static void RunGame(Player player)
    {
        var rooms = new List<Room>
        {
            new("battle", "Skogsstig"),
            new("treasure", "Gammal kista"),
            new("shop", "Vandrande köpman"),
            new("battle", "Grottans mynning"),
            new("rest", "Lägereld"),
            new("battle", "Grottans djup"),
            new("boss", "Urdraken")
        };

        var battle = new BattleEngine();
        var handler = new RoomHandler();

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];
            Console.WriteLine($"--- Rum {i + 1}/{rooms.Count}: {room.Label} ({room.Type}) ---");

            bool continueAdventure = room.Type switch
            {
                "battle" => battle.DoBattle(
                    player,
                    new Enemy("Goblin", 25, 6, 2, xpReward: 10, goldReward: 5),
                    isBoss: false),
                "boss" => battle.DoBattle(
                    player,
                    new Enemy("Urdraken", 60, 10, 4, xpReward: 50, goldReward: 20),
                    isBoss: true),
                "treasure" => handler.HandleRoom(player, room),
                "shop" => handler.HandleRoom(player, room),
                "rest" => handler.HandleRoom(player, room),
                _ => true
            };

            if (!continueAdventure || player.IsDead)
            {
                Console.WriteLine("Äventyret är över.");
                return;
            }
        }

        Console.WriteLine("Du klarade hela äventyret.");
    }
}
