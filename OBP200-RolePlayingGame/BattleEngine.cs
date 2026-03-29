namespace OBP200_RolePlayingGame;

public class BattleEngine
{
    private readonly Random _rng = new();

    public bool DoBattle(Player player, Enemy enemy, bool isBoss)
    {
        Console.WriteLine(
            $"En {enemy.Name} dyker upp! (HP {enemy.Hp}, ATK {enemy.AttackPower}, DEF {enemy.Defense})");

        while (!player.IsDead && !enemy.IsDead)
        {
            Console.WriteLine();
            ShowStatus(player, enemy);

            Console.WriteLine("[A] Attack  [P] Dryck  [R] Fly");
            if (isBoss)
                Console.WriteLine("(Du kan inte fly från en boss!)");

            Console.Write("Val: ");
            var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (cmd == "A")
            {
                int dmg = CalculatePlayerDamage(player, enemy.Defense);
                enemy.TakeDamage(dmg);
                Console.WriteLine($"Du slog {enemy.Name} för {dmg} skada.");
            }
            else if (cmd == "P")
            {
                if (!player.Inventory.UsePotion(player))
                    Console.WriteLine("Du har inga drycker kvar.");
                else
                    Console.WriteLine("Du dricker en dryck.");
            }
            else if (cmd == "R" && !isBoss)
            {
                if (TryRunAway(player))
                {
                    Console.WriteLine("Du flydde!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Misslyckad flykt!");
                }
            }
            else
            {
                Console.WriteLine("Du tvekar...");
            }

            if (enemy.IsDead)
                break;

            int enemyDmg = CalculateEnemyDamage(player, enemy.AttackPower);
            player.TakeDamage(enemyDmg);
            Console.WriteLine($"{enemy.Name} anfaller och gör {enemyDmg} skada!");
        }

        if (player.IsDead)
            return false;

        player.AddXp(enemy.XpReward);
        player.AddGold(enemy.GoldReward);
        Console.WriteLine($"Seger! +{enemy.XpReward} XP, +{enemy.GoldReward} guld.");

        MaybeDropLoot(player, enemy.Name);

        return true;
    }

    private int CalculatePlayerDamage(Player player, int enemyDef)
    {
        int baseDmg = Math.Max(1, player.AttackPower - (enemyDef / 2));
        int roll = _rng.Next(0, 3);

        switch (player.ClassName)
        {
            case "Warrior":
                baseDmg += 1;
                break;
            case "Mage":
                baseDmg += 2;
                break;
            case "Rogue":
                if (_rng.NextDouble() < 0.2)
                    baseDmg += 4;
                break;
        }

        return Math.Max(1, baseDmg + roll);
    }

    private int CalculateEnemyDamage(Player player, int enemyAtk)
    {
        int roll = _rng.Next(0, 3);
        int dmg = Math.Max(1, enemyAtk - (player.Defense / 2)) + roll;

        if (_rng.NextDouble() < 0.1)
            dmg = Math.Max(1, dmg - 2);

        return dmg;
    }

    private bool TryRunAway(Player player)
    {
        double chance = player.ClassName switch
        {
            "Rogue" => 0.5,
            "Mage" => 0.35,
            _ => 0.25
        };

        return _rng.NextDouble() < chance;
    }

    private void MaybeDropLoot(Player player, string enemyName)
    {
        if (_rng.NextDouble() < 0.35)
        {
            string item = enemyName.Contains("Urdraken")
                ? "Dragon Scale"
                : "Minor Gem";

            player.Inventory.AddItem(item);
            Console.WriteLine($"Föremål hittat: {item} (lagt i din väska)");
        }
    }

    private void ShowStatus(Player player, Enemy enemy)
    {
        Console.WriteLine(
            $"[{player.Name} | {player.ClassName}] HP {player.Hp}/{player.MaxHp} ATK {player.AttackPower} DEF {player.Defense} LVL {player.Level} XP {player.Xp} Guld {player.Gold} Drycker {player.Inventory.Potions}");

        if (player.Inventory.Items.Any())
            Console.WriteLine($"Väska: {string.Join("; ", player.Inventory.Items)}");

        Console.WriteLine($"Fiende: {enemy.Name} HP={enemy.Hp}/{enemy.MaxHp}");
    }
}
