namespace RpgGame.Models.GameComponent.GameActions.Skills;

public abstract class Skill : GameAction
{
    protected Skill(string name, int mpCost, TargetType targetType)
        : base(name, mpCost, targetType) { }


}
