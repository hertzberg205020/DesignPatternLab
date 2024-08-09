using TreasureMap.Models.Roles;

namespace TreasureMap.Models.MapComponents;

public abstract class MapObject
{
    public GameMap? GameMap { get; set; }
    public abstract char Symbol { get; }

    public Coordinate? Position { get; set; }

    public virtual void BeTouched(Role role) { }
}
