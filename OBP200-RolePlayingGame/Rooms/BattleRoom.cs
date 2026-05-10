namespace OBP200_RolePlayingGame.Rooms;

using Characters;
using Combat;

/// <summary>
/// A room that spawns an enemy (or the boss) and runs a combat encounter.
/// </summary>
public class BattleRoom : Room
{
    private readonly bool _isBoss;
    private readonly Random _rng;

    public BattleRoom(string label, bool isBoss, Random rng) : base(label)
    {
        _isBoss = isBoss;
        _rng = rng;
    }

    public override bool Enter(Player player)
    {
        var enemy = _isBoss
            ? EnemyFactory.CreateBoss()
            : EnemyFactory.CreateRandom(_rng);

        return CombatSystem.RunCombat(player, enemy, _isBoss, _rng);
    }
}

