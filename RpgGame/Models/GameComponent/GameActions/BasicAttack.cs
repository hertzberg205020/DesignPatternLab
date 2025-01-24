using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions;

public class BasicAttack : GameAction
{
    public BasicAttack(int mpCost = 0)
        : base("普通攻擊", mpCost, TargetType.Enemy) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 1;

    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        var target = targets.Single();
        executant.Attack(target, executant.Strength);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        var target = targets.Single();
        // [1]英雄 攻擊 [2]Slime1。
        return $"{executant.Name} 攻擊 {target.Name}。";
    }
}
