namespace OBP200_RolePlayingGame;

public class Inventory
{
    public int Potions { get; private set; }
    public List<string> Items { get; } = new();

    public Inventory(int potions)
    {
        Potions = potions;
    }

    public bool UsePotion(Player player)
    {
        if (Potions <= 0) return false;

        Potions--;
        player.Heal(12);
        return true;
    }

    public void AddItem(string item)
    {
        if (!string.IsNullOrWhiteSpace(item))
            Items.Add(item);
    }

    public int SellAll(string itemName, int goldPerItem)
    {
        int count = Items.Count(i => i == itemName);
        if (count == 0) return 0;

        Items.RemoveAll(i => i == itemName);
        return count * goldPerItem;
    }
}