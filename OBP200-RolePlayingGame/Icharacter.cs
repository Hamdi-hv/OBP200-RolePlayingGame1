namespace OBP200_RolePlayingGame;

public interface ICharacter
{
    string Name { get; }
    int Hp { get; }
    int MaxHp { get; }
    int AttackPower { get; }
    int Defense { get; }
    bool IsDead { get; }

    int Attack();
    void TakeDamage(int amount);
    void Heal(int amount);
}