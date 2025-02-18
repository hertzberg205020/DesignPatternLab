using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public abstract class Skill : GameAction
{
    protected Skill(string name, int mpCost, TargetType targetType, IGameIO gameIO)
        : base(name, mpCost, targetType, gameIO) { }
}
