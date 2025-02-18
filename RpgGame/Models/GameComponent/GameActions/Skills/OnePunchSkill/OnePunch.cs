using RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkill.OnePunchSkillHandler;
using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkill;

public class OnePunch : Skill
{
    private readonly OnePunchHandler _handler;

    public OnePunch(OnePunchHandler handler, IGameIO gameIO)
        : base("一拳攻擊", 180, TargetType.Enemy, gameIO)
    {
        _handler = handler;
    }

    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(self);

        return 1;
    }

    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        var target = targets.Single();

        _handler.Handle(executant, target);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        var target = targets.Single();

        // [1]英雄 對 [2]Slime1 使用了 一拳攻擊。
        return $"{executant} 對 {target} 使用了 一拳攻擊。";
    }
}
