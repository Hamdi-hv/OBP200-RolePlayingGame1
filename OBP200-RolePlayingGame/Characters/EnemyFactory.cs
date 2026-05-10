namespace OBP200_RolePlayingGame.Characters;

/// <summary>
/// Creates enemy instances. Keeps template data and randomisation in one place (SRP).
/// </summary>
public static class EnemyFactory
{
    private static readonly (string Type, string Name, int Hp, int Atk, int Def, int Xp, int Gold)[] Templates =
    {
        ("beast",  "Vildsvin",  18, 4, 1, 6, 4),
        ("undead", "Skelett",   20, 5, 2, 7, 5),
        ("bandit", "Bandit",    16, 6, 1, 8, 6),
        ("slime",  "Geléslem",  14, 3, 0, 5, 3),
    };

    public static Enemy CreateRandom(Random rng)
    {
        var t = Templates[rng.Next(Templates.Length)];
        return new Enemy(
            t.Type, t.Name,
            t.Hp   + rng.Next(-1, 3),
            t.Atk  + rng.Next(0, 2),
            t.Def  + rng.Next(0, 2),
            t.Xp   + rng.Next(0, 3),
            t.Gold + rng.Next(0, 3));
    }

    public static Enemy CreateBoss() =>
        new Enemy("boss", "Urdraken", 55, 9, 4, 30, 50);
}

