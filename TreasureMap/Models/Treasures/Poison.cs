using TreasureMap.Models.Roles;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Treasures;

public class Poison : Treasure, ITreasure
{
    public static double Probability => 0.25;

    public static string Name => "毒藥";

    protected override void ApplyEffect(Role role)
    {
        role.EnterState(new Poisoned());
    }
}
