using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.GameActions;

public class BasicAttack : GameAction
{
    public BasicAttack(IGameIO gameIO)
        : base("普通攻擊", 0, TargetType.Enemy, gameIO) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 1;

    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        if (targets.Count != 1)
        {
            throw new InvalidOperationException("BasicAttack requires exactly one target.");
        }

        var target = targets.Single();
        executant.Attack(target, executant.Strength);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        var target = targets.Single();
        // [1]英雄 攻擊 [2]Slime1。
        return $"{executant} 攻擊 {target}。";
    }
}
