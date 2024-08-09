using TreasureMap.Models.Roles;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Treasures;

public class KingRock : Treasure, ITreasure
{
    public static double Probability => 0.1;

    public static string Name => "王者之印";

    protected override void ApplyEffect(Role role)
    {
        role.EnterState(new Stockpile());
    }
}
