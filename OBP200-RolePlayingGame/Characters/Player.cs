namespace OBP200_RolePlayingGame.Characters;

/// <summary>
/// Represents the player character with inventory, gold, XP and level progression.
/// </summary>
public class Player : Character
{
    private readonly List<string> _inventory;

    public PlayerClass Class { get; }
    public int Gold { get; private set; }
    public int Xp { get; private set; }
    public int Level { get; private set; }
    public int Potions { get; private set; }
    public IReadOnlyList<string> Inventory => _inventory.AsReadOnly();

    public Player(string name, PlayerClass playerClass, int maxHp, int attack, int defense, int gold, int potions)
        : base(name, maxHp, attack, defense)
    {
        Class = playerClass;
        Gold = gold;
        Potions = potions;
        Level = 1;
        Xp = 0;
        _inventory = new List<string> { "Wooden Sword", "Cloth Armor" };
    }

    // ── Gold ─────────────────────────────────────────────────────────────

    public void AddGold(int amount) => Gold += Math.Max(0, amount);

    /// <summary>Returns true and deducts gold if the player can afford <paramref name="cost"/>.</summary>
    public bool SpendGold(int cost)
    {
        if (Gold < cost) return false;
        Gold -= cost;
        return true;
    }

    // ── XP & levelling ───────────────────────────────────────────────────

    public void AddXp(int amount)
    {
        Xp += Math.Max(0, amount);
        TryLevelUp();
    }

    private void TryLevelUp()
    {
        int threshold = Level switch
        {
            1 => 10,
            2 => 25,
            3 => 45,
            _ => Level * 20
        };

        if (Xp < threshold) return;

        Level++;

        switch (Class)
        {
            case PlayerClass.Warrior: MaxHp += 6; Attack += 2; Defense += 2; break;
            case PlayerClass.Mage:    MaxHp += 4; Attack += 4; Defense += 1; break;
            case PlayerClass.Rogue:   MaxHp += 5; Attack += 3; Defense += 1; break;
        }

        Hp = MaxHp; // full heal on level-up
        Console.WriteLine($"Du når nivå {Level}! Värden ökade och HP återställd.");
    }

    // ── Combat helpers ───────────────────────────────────────────────────

    /// <summary>Uses one potion. Returns false if none are available.</summary>
    public bool UsePotion()
    {
        if (Potions <= 0) return false;

        int healed = Math.Min(MaxHp - Hp, 12);
        Hp += healed;
        Potions--;
        Console.WriteLine($"Du dricker en dryck och återfår {healed} HP.");
        return true;
    }

    public void RestoreFullHp() => Hp = MaxHp;

    // ── Shop helpers ─────────────────────────────────────────────────────

    public void AddPotion() => Potions++;
    public void UpgradeAttack(int amount) => Attack += amount;
    public void UpgradeDefense(int amount) => Defense += amount;

    // ── Inventory ────────────────────────────────────────────────────────

    public void AddItem(string item) => _inventory.Add(item);

    /// <summary>
    /// Removes all items with the given name and returns the count removed.
    /// Returns false if none were found.
    /// </summary>
    public bool RemoveItems(string itemName, out int count)
    {
        count = _inventory.Count(i => i == itemName);
        if (count == 0) return false;
        _inventory.RemoveAll(i => i == itemName);
        return true;
    }

    // ── Display ──────────────────────────────────────────────────────────

    public void ShowStatus()
    {
        Console.WriteLine(
            $"[{Name} | {Class}]  HP {Hp}/{MaxHp}  ATK {Attack}  DEF {Defense}" +
            $"  LVL {Level}  XP {Xp}  Guld {Gold}  Drycker {Potions}");

        if (_inventory.Count > 0)
            Console.WriteLine($"Väska: {string.Join("; ", _inventory)}");
    }
}

