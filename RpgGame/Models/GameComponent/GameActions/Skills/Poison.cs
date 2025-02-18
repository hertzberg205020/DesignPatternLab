using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class Poison : Skill
{
    public Poison(IGameIO gameIO)
        : base("下毒", 80, TargetType.Enemy, gameIO) { }

    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(self, nameof(self));

        return 1;
    }

    /// <summary>
    /// 使目標角色獲得三回合的中毒狀態 (包含當前回合)
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        var target = targets.Single();
        target.EnterState(new PoisonedState(GameIO));
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        var target = targets.Single();
        // [1]英雄 對 [2]Slime1 使用了 下毒。
        return $"{executant} 對 {target} 使用了 {Name}。";
    }
}
