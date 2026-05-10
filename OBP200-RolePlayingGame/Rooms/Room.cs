namespace OBP200_RolePlayingGame.Rooms;

using Characters;

/// <summary>
/// Abstract base for all room types. Subclasses implement their own behaviour (OCP).
/// </summary>
public abstract class Room
{
    public string Label { get; }

    protected Room(string label)
    {
        Label = label;
    }

    /// <summary>
    /// Runs the room event for the player.
    /// Returns <c>true</c> to continue the adventure, <c>false</c> to end it.
    /// </summary>
    public abstract bool Enter(Player player);
}

