using TreasureMap.Models.Roles;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Treasures;

public class DevilFruit : Treasure, ITreasure
{
    public static double Probability => 0.1;

    public static string Name => "惡魔果實";

    protected override void ApplyEffect(Role role)
    {
        role.EnterState(new Orderless(role));
    }
}
