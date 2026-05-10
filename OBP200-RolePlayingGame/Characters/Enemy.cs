namespace OBP200_RolePlayingGame.Characters;

/// <summary>
/// Represents an enemy with combat rewards.
/// </summary>
public class Enemy : Character
{
    public string EnemyType { get; }
    public int XpReward { get; }
    public int GoldReward { get; }

    public Enemy(string enemyType, string name, int maxHp, int attack, int defense, int xpReward, int goldReward)
        : base(name, maxHp, attack, defense)
    {
        EnemyType = enemyType;
        XpReward = xpReward;
        GoldReward = goldReward;
    }
}

