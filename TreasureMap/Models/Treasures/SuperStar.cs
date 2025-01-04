using TreasureMap.Models.Roles;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Treasures;

public class SuperStar : Treasure, ITreasure
{
    public static double Probability => 0.1;
    public static string Name => "無敵星星";

    protected override void ApplyEffect(Role role)
    {
        role.EnterState(new Invincible(role));
    }
}
