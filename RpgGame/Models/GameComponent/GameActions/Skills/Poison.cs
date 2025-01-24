using RpgGame.Models.GameComponent.States;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class Poison : Skill
{
    public Poison()
        : base("下毒", 80, TargetType.Enemy) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 1;

    /// <summary>
    /// 使目標角色獲得三回合的中毒狀態 (包含當前回合)
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        var target = targets.Single();
        target.EnterState(new PoisonedState(target));
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        var target = targets.Single();
        // [1]英雄 對 [2]Slime1 使用了 下毒。
        return $"{executant.Name} 對 {target.Name} 使用了 {Name}。";
    }
}
