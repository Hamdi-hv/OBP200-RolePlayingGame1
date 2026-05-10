namespace OBP200_RolePlayingGame;

using System.Text;
using Characters;
using Rooms;

/// <summary>
/// Manages the main game loop, room progression and player creation.
/// </summary>
public class Game
{
    private readonly Random _rng = new();
    private Player? _player;
    private List<Room> _rooms = new();
    private int _currentRoomIndex;

    public void Run()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            ShowMainMenu();
            Console.Write("Välj: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1":
                    StartNewGame();
                    RunGameLoop();
                    break;
                case "2":
                    Console.WriteLine("Avslutar...");
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }

            Console.WriteLine();
        }
    }

    // ── Menus ─────────────────────────────────────────────────────────────

    private static void ShowMainMenu()
    {
        Console.WriteLine("=== Text-RPG ===");
        Console.WriteLine("1. Nytt spel");
        Console.WriteLine("2. Avsluta");
    }

    // ── Game setup ────────────────────────────────────────────────────────

    private void StartNewGame()
    {
        Console.Write("Ange namn: ");
        var name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name)) name = "Namnlös";

        Console.WriteLine("Välj klass: 1) Warrior  2) Mage  3) Rogue");
        Console.Write("Val: ");
        var k = (Console.ReadLine() ?? "").Trim();

        _player = k switch
        {
            "2" => new Player(name, PlayerClass.Mage,    maxHp: 28, attack: 10, defense: 2,  gold: 15, potions: 2),
            "3" => new Player(name, PlayerClass.Rogue,   maxHp: 32, attack: 8,  defense: 3,  gold: 20, potions: 3),
            _   => new Player(name, PlayerClass.Warrior, maxHp: 40, attack: 7,  defense: 5,  gold: 15, potions: 2),
        };

        InitRooms();
        _currentRoomIndex = 0;

        Console.WriteLine($"Välkommen, {_player.Name} the {_player.Class}!");
        _player.ShowStatus();
    }

    private void InitRooms()
    {
        _rooms = new List<Room>
        {
            new BattleRoom ("Skogsstig",         isBoss: false, _rng),
            new TreasureRoom("Gammal kista",      _rng),
            new ShopRoom   ("Vandrande köpman"),
            new BattleRoom ("Grottans mynning",  isBoss: false, _rng),
            new RestRoom   ("Lägereld"),
            new BattleRoom ("Grottans djup",     isBoss: false, _rng),
            new BattleRoom ("Urdraken",          isBoss: true,  _rng),
        };
    }

    // ── Game loop ─────────────────────────────────────────────────────────

    private void RunGameLoop()
    {
        while (true)
        {
            var room = _rooms[_currentRoomIndex];
            Console.WriteLine($"\n--- Rum {_currentRoomIndex + 1}/{_rooms.Count}: {room.Label} ---");

            bool continueAdventure = room.Enter(_player!);

            if (!_player!.IsAlive)
            {
                Console.WriteLine("Du har stupat... Spelet över.");
                break;
            }

            if (!continueAdventure)
            {
                Console.WriteLine("Du lämnar äventyret för nu.");
                break;
            }

            _currentRoomIndex++;

            if (_currentRoomIndex >= _rooms.Count)
            {
                Console.WriteLine("\nDu har klarat äventyret!");
                break;
            }

            Console.WriteLine("\n[C] Fortsätt     [Q] Avsluta till huvudmeny");
            Console.Write("Val: ");
            var post = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (post == "Q")
            {
                Console.WriteLine("Tillbaka till huvudmenyn.");
                break;
            }
        }
    }
}