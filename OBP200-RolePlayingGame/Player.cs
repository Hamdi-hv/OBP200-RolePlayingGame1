namespace OBP200_RolePlayingGame;

public class Player : ICharacter
{
    public string Name { get; private set; }
    public string ClassName { get; private set; }

    public int Hp { get; private set; }
    public int MaxHp { get; private set; }
    public int AttackPower { get; private set; }
    public int Defense { get; private set; }

    public int Gold { get; private set; }
    public int Xp { get; private set; }
    public int Level { get; private set; } = 1;

    public Inventory Inventory { get; private set; }

    public bool IsDead => Hp <= 0;

    public Player(string name, string className, int maxHp, int atk, int def, int gold, int potions)
    {
        Name = name;
        ClassName = className;
        MaxHp = maxHp;
        Hp = maxHp;
        AttackPower = atk;
        Defense = def;
        Gold = gold;
        Inventory = new Inventory(potions);
    }

    public int Attack() => AttackPower;

    public void TakeDamage(int amount)
    {
        Hp = Math.Max(0, Hp - Math.Max(0, amount));
    }

    public void Heal(int amount)
    {
        Hp = Math.Min(MaxHp, Hp + Math.Max(0, amount));
    }

    public void AddGold(int amount) => Gold += Math.Max(0, amount);

    public bool SpendGold(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        return true;
    }

    public void AddXp(int amount)
    {
        Xp += Math.Max(0, amount);
        MaybeLevelUp();
    }

    private void MaybeLevelUp()
    {
        int nextThreshold = Level == 1 ? 10 :
                            Level == 2 ? 25 :
                            Level == 3 ? 45 :
                            Level * 20;

        if (Xp < nextThreshold) return;

        Level++;

        switch (ClassName)
        {
            case "Warrior":
                MaxHp += 6;
                AttackPower += 2;
                Defense += 2;
                break;
            case "Mage":
                MaxHp += 4;
                AttackPower += 4;
                Defense += 1;
                break;
            case "Rogue":
                MaxHp += 5;
                AttackPower += 3;
                Defense += 1;
                break;
        }

        Hp = MaxHp;
    }
}
