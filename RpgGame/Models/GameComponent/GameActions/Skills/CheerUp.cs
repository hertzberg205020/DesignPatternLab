using RpgGame.Models.GameComponent.States;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class CheerUp : Skill
{
    public CheerUp()
        : base("鼓舞", 100, TargetType.Ally) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 3;

    /// <summary>
    /// 使所有目標角色獲得三回合的受到鼓舞狀態 (包含當前回合)
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        foreach (var target in targets)
        {
            target.EnterState(new CheerUpState(target));
        }
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant);
        ArgumentNullException.ThrowIfNull(targets);

        var targetNames = targets.Select(t => t.Name).ToList();

        // [1]英雄 對 [1]Servant1, [1]Servant2, [1]Servant3 使用了 鼓舞。
        return $"{executant.Name} 對 {string.Join(", ", targetNames)} 使用了 {Name}。";
    }
}
