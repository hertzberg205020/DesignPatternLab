using RpgGame.Models.GameComponent.States;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class SelfHealing : Skill
{
    public SelfHealing()
        : base("自我治療", 50, TargetType.Self) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 0;

    /// <summary>
    /// 增加自己 150 點 HP
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game);

        ArgumentNullException.ThrowIfNull(executant);

        executant.TakeHeal(150);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        // [2]Slime1 使用了 自我治療。
        return $"{executant.Name} 使用了 {Name}。";
    }
}
