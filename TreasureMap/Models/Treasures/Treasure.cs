using TreasureMap.Models.MapComponents;
using TreasureMap.Models.Roles;

namespace TreasureMap.Models.Treasures;

public abstract class Treasure : MapObject
{
    public override char Symbol => 'x';

    public override void BeTouched(Role role)
    {
        this.ApplyEffect(role);
    }

    protected abstract void ApplyEffect(Role role);
}

/// <summary>
/// Represents a probability of a treasure being found.
/// </summary>

public interface ITreasure
{
    static abstract double Probability { get; }

    static abstract string Name { get; }
}
