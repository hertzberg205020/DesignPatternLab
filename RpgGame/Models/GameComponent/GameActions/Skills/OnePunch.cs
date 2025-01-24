using RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkillHandler;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class OnePunch : Skill
{
    private readonly OnePunchHandler _handler;

    public OnePunch(OnePunchHandler handler)
        : base("一拳攻擊", 180, TargetType.Self)
    {
        _handler = handler;
    }

    public override int GetRequiredTargetCount(Game game, Role self) => 1;

    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        var target = targets.Single();

        _handler.Handle(executant, target);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        var target = targets.Single();

        // [1]英雄 對 [2]Slime1 使用了 一拳攻擊。
        return $"{executant.Name} 對 {target.Name} 使用了 一拳攻擊。";
    }
}
