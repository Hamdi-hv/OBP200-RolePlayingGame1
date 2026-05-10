namespace OBP200_RolePlayingGame.Rooms;

using Characters;

/// <summary>
/// A room containing a treasure chest with gold or an item inside.
/// </summary>
public class TreasureRoom : Room
{
    private static readonly string[] PossibleItems =
        { "Iron Dagger", "Oak Staff", "Leather Vest", "Healing Herb" };

    private readonly Random _rng;

    public TreasureRoom(string label, Random rng) : base(label)
    {
        _rng = rng;
    }

    public override bool Enter(Player player)
    {
        Console.WriteLine("Du hittar en gammal kista...");

        if (_rng.NextDouble() < 0.5)
        {
            int gold = _rng.Next(8, 15);
            player.AddGold(gold);
            Console.WriteLine($"Kistan innehåller {gold} guld!");
        }
        else
        {
            string item = PossibleItems[_rng.Next(PossibleItems.Length)];
            player.AddItem(item);
            Console.WriteLine($"Du plockar upp: {item}");
        }

        return true;
    }
}

