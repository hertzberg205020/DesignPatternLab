using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class Petrochemical : Skill
{
    public Petrochemical(IGameIO gameIO)
        : base("石化", 100, TargetType.Enemy, gameIO) { }

    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(game);

        ArgumentNullException.ThrowIfNull(self);

        return 1;
    }

    /// <summary>
    /// 使目標角色獲得三回合的石化狀態
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game);

        ArgumentNullException.ThrowIfNull(executant);

        ArgumentNullException.ThrowIfNull(targets);

        var target = targets.Single();
        target.EnterState(new PetrifiedState(GameIO));
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant);

        ArgumentNullException.ThrowIfNull(targets);

        var target = targets.Single();
        // [1]英雄 對 [2]攻擊力超強的BOSS 使用了 石化。
        return $"{executant} 對 {target} 使用了 {Name}。";
    }
}
