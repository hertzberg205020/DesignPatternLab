using TreasureMap.Models.Roles;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Treasures;

public class DokodemoDoor : Treasure, ITreasure
{
    public static double Probability => 0.1;

    public static string Name => "任意門";

    protected override void ApplyEffect(Role role)
    {
        role.EnterState(new Teleport(role));
    }
}
