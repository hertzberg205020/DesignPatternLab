using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class Curse : Skill
{
    public Curse()
        : base("詛咒", 100, TargetType.Enemy) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 1;

    /// <summary>
    /// 目標角色會受到來自於行動角色的詛咒。
    /// 如果詛咒是來自於同一位施咒者，效果並不會疊加。
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        var target = targets.Single();

        // check if the target is already being cursed by the executant
        if (
            target
                .GetDeathObservers()
                .Any(observer =>
                    observer is CurseRelationship curseRelation && curseRelation.Caster == executant
                )
        )
        {
            return;
        }

        var curseRelation = new CurseRelationship(executant, target);

        target.RegisterDeathObserver(curseRelation);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        var targetName = targets.Single().Name;

        // [1]英雄 對 [2]Slime2 使用了 詛咒。
        return $"{executant.Name} 對 {targetName} 使用了 {Name}。";
    }
}
