using TreasureMap.Models.Roles;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Treasures;

public class HealingPotion : Treasure, ITreasure
{
    public static double Probability => 0.15;

    public static string Name => "補血罐";

    protected override void ApplyEffect(Role role)
    {
        role.EnterState(new Healing());
    }
}
