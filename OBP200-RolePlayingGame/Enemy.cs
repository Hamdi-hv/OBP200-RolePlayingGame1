namespace OBP200_RolePlayingGame;

public class Enemy : ICharacter
{
    public string Name { get; private set; }

    public int Hp { get; private set; }
    public int MaxHp { get; private set; }
    public int AttackPower { get; private set; }
    public int Defense { get; private set; }

    public int XpReward { get; private set; }
    public int GoldReward { get; private set; }

    public bool IsDead => Hp <= 0;

    public Enemy(string name, int hp, int atk, int def, int xpReward, int goldReward)
    {
        Name = name;
        MaxHp = hp;
        Hp = hp;
        AttackPower = atk;
        Defense = def;
        XpReward = xpReward;
        GoldReward = goldReward;
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
}