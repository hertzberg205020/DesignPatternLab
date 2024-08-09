using TreasureMap.Models.Roles;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Treasures;

public class AcceleratingPotion : Treasure, ITreasure
{
    public static double Probability => 0.2;

    public static string Name => "加速藥水";

    protected override void ApplyEffect(Role role)
    {
        role.EnterState(new Accelerated());
    }
}
