namespace OBP200_RolePlayingGame.Rooms;

using Characters;

/// <summary>
/// A campfire rest room that fully restores the player's HP.
/// </summary>
public class RestRoom : Room
{
    public RestRoom(string label) : base(label) { }

    public override bool Enter(Player player)
    {
        Console.WriteLine("Du slår läger och vilar.");
        player.RestoreFullHp();
        Console.WriteLine("HP återställt till max.");
        return true;
    }
}

