namespace OBP200_RolePlayingGame.Combat;

using Characters;

/// <summary>
/// Handles all combat logic. Separated from characters and rooms (SRP).
/// </summary>
public static class CombatSystem
{
    /// <summary>
    /// Runs a full combat loop between the player and an enemy.
    /// Returns <c>true</c> to continue the adventure, <c>false</c> if the player died.
    /// </summary>
    public static bool RunCombat(Player player, Enemy enemy, bool isBoss, Random rng)
    {
        Console.WriteLine($"En {enemy.Name} dyker upp! (HP {enemy.Hp}, ATK {enemy.Attack}, DEF {enemy.Defense})");

        while (enemy.IsAlive && player.IsAlive)
        {
            Console.WriteLine();
            player.ShowStatus();
            Console.WriteLine($"Fiende: {enemy.Name} HP={enemy.Hp}");
            Console.WriteLine("[A] Attack   [X] Special   [P] Dryck   [R] Fly");
            if (isBoss) Console.WriteLine("(Du kan inte fly från en boss!)");
            Console.Write("Val: ");

            var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            switch (cmd)
            {
                case "A":
                    int dmg = CalculatePlayerDamage(player, enemy.Defense, rng);
                    enemy.TakeDamage(dmg);
                    Console.WriteLine($"Du slog {enemy.Name} för {dmg} skada.");
                    break;

                case "X":
                    int specialDmg = UseClassSpecial(player, enemy.Defense, isBoss, rng);
                    enemy.TakeDamage(specialDmg);
                    Console.WriteLine($"Special! {enemy.Name} tar {specialDmg} skada.");
                    break;

                case "P":
                    if (!player.UsePotion())
                        Console.WriteLine("Du har inga drycker kvar.");
                    break;

                case "R":
                    if (isBoss)
                    {
                        Console.WriteLine("Du kan inte fly från en boss!");
                    }
                    else if (TryFlee(player, rng))
                    {
                        Console.WriteLine("Du flydde!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Misslyckad flykt!");
                    }
                    break;

                default:
                    Console.WriteLine("Du tvekar...");
                    break;
            }

            if (!enemy.IsAlive) break;

            int enemyDmg = CalculateEnemyDamage(enemy.Attack, player.Defense, rng);
            player.TakeDamage(enemyDmg);
            Console.WriteLine($"{enemy.Name} anfaller och gör {enemyDmg} skada!");
        }

        if (!player.IsAlive) return false;

        player.AddXp(enemy.XpReward);
        player.AddGold(enemy.GoldReward);
        Console.WriteLine($"Seger! +{enemy.XpReward} XP, +{enemy.GoldReward} guld.");
        MaybeDropLoot(player, enemy.Name, rng);

        return true;
    }

    // ── Damage calculations ───────────────────────────────────────────────

    private static int CalculatePlayerDamage(Player player, int enemyDef, Random rng)
    {
        int baseDmg = Math.Max(1, player.Attack - (enemyDef / 2));

        baseDmg += player.Class switch
        {
            PlayerClass.Warrior => 1,
            PlayerClass.Mage    => 2,
            PlayerClass.Rogue   => rng.NextDouble() < 0.2 ? 4 : 0,
            _                   => 0
        };

        return Math.Max(1, baseDmg + rng.Next(0, 3));
    }

    private static int UseClassSpecial(Player player, int enemyDef, bool vsBoss, Random rng)
    {
        int specialDmg;

        switch (player.Class)
        {
            case PlayerClass.Warrior:
                Console.WriteLine("Warrior använder Heavy Strike!");
                specialDmg = Math.Max(2, player.Attack + 3 - enemyDef);
                player.TakeDamage(2); // self-damage
                break;

            case PlayerClass.Mage:
                if (player.SpendGold(3))
                {
                    Console.WriteLine("Mage kastar Fireball!");
                    specialDmg = Math.Max(3, player.Attack + 5 - (enemyDef / 2));
                }
                else
                {
                    Console.WriteLine("Inte tillräckligt med guld för Fireball (kostar 3).");
                    specialDmg = 0;
                }
                break;

            case PlayerClass.Rogue:
                if (rng.NextDouble() < 0.5)
                {
                    Console.WriteLine("Rogue utför en lyckad Backstab!");
                    specialDmg = Math.Max(4, player.Attack + 6);
                }
                else
                {
                    Console.WriteLine("Backstab misslyckades!");
                    specialDmg = 1;
                }
                break;

            default:
                specialDmg = 0;
                break;
        }

        if (vsBoss) specialDmg = (int)Math.Round(specialDmg * 0.8);
        return Math.Max(0, specialDmg);
    }

    private static int CalculateEnemyDamage(int enemyAtk, int playerDef, Random rng)
    {
        int dmg = Math.Max(1, enemyAtk - (playerDef / 2)) + rng.Next(0, 3);
        if (rng.NextDouble() < 0.1) dmg = Math.Max(1, dmg - 2); // glancing blow
        return dmg;
    }

    // ── Flee & loot ───────────────────────────────────────────────────────

    private static bool TryFlee(Player player, Random rng)
    {
        double chance = player.Class switch
        {
            PlayerClass.Rogue => 0.5,
            PlayerClass.Mage  => 0.35,
            _                 => 0.25
        };
        return rng.NextDouble() < chance;
    }

    private static void MaybeDropLoot(Player player, string enemyName, Random rng)
    {
        if (rng.NextDouble() >= 0.35) return;

        string item = enemyName.Contains("Urdraken") ? "Dragon Scale" : "Minor Gem";
        player.AddItem(item);
        Console.WriteLine($"Föremål hittat: {item} (lagt i din väska)");
    }
}

