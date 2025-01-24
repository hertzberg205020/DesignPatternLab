using RpgGame.Models.GameComponent.States;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class Petrochemical : Skill
{
    public Petrochemical()
        : base("石化", 100, TargetType.Enemy) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 1;

    /// <summary>
    /// 使目標角色獲得三回合的石化狀態
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game);

        ArgumentNullException.ThrowIfNull(executant);

        ArgumentNullException.ThrowIfNull(targets);

        var target = targets.Single();
        target.EnterState(new PetrifiedState(target));
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant);

        ArgumentNullException.ThrowIfNull(targets);

        var target = targets.Single();
        // [1]英雄 對 [2]攻擊力超強的BOSS 使用了 石化。
        return $"{executant.Name} 對 {target.Name} 使用了 {Name}!";
    }
}
