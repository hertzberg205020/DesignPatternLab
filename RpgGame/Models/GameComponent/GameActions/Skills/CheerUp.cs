using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class CheerUp : Skill
{
    public CheerUp(IGameIO gameIO)
        : base("鼓舞", 100, TargetType.Ally, gameIO) { }

    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(self);

        return 3;
    }

    /// <summary>
    /// 使所有目標角色獲得三回合的受到鼓舞狀態 (包含當前回合)
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        foreach (var target in targets)
        {
            target.EnterState(new CheerUpState(GameIO));
        }
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant);
        ArgumentNullException.ThrowIfNull(targets);

        if (targets.Count == 0)
        {
            // [2]Slime3 使用了 鼓舞。
            return $"{executant} 使用了 {Name}。";
        }

        // [1]英雄 對 [1]Servant1, [1]Servant2, [1]Servant3 使用了 鼓舞。
        return $"{executant} 對 {string.Join(", ", targets)} 使用了 {Name}。";
    }
}
