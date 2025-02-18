using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class SelfHealing : Skill
{
    public SelfHealing(IGameIO gameIO)
        : base("自我治療", 50, TargetType.Self, gameIO) { }

    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(game);

        ArgumentNullException.ThrowIfNull(self);

        return 0;
    }

    /// <summary>
    /// 增加自己 150 點 HP
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game);

        ArgumentNullException.ThrowIfNull(executant);

        executant.TakeHeal(150);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        // [2]Slime1 使用了 自我治療。
        return $"{executant} 使用了 {Name}。";
    }
}
