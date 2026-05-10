namespace OBP200_RolePlayingGame.Interfaces;

/// <summary>
/// Represents any entity that can participate in combat.
/// </summary>
public interface ICombatant
{
    string Name { get; }
    int Hp { get; }
    int MaxHp { get; }
    int Attack { get; }
    int Defense { get; }
    bool IsAlive { get; }
    void TakeDamage(int damage);
}

