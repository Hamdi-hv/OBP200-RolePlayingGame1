namespace OBP200_RolePlayingGame.Characters;

using Interfaces;

/// <summary>
/// Abstract base class for all combatants. Provides shared state and behaviour.
/// </summary>
public abstract class Character : ICombatant
{
    private int _hp;

    public string Name { get; protected set; }

    public int Hp
    {
        get => _hp;
        protected set => _hp = Math.Max(0, value);
    }

    public int MaxHp { get; protected set; }
    public int Attack { get; protected set; }
    public int Defense { get; protected set; }
    public bool IsAlive => Hp > 0;

    protected Character(string name, int maxHp, int attack, int defense)
    {
        Name = name;
        MaxHp = maxHp;
        Attack = attack;
        Defense = defense;
        Hp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        Hp -= Math.Max(0, damage);
    }

    public override string ToString() =>
        $"{Name} HP:{Hp}/{MaxHp} ATK:{Attack} DEF:{Defense}";
}

