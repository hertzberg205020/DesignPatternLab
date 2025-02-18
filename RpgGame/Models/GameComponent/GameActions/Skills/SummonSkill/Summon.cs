using RpgGame.Models.GameComponent.DecisionStrategies;
using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills.SummonSkill;

public class Summon : Skill
{
    public Summon(IGameIO gameIO)
        : base("召喚", 150, TargetType.None, gameIO) { }

    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(self, nameof(self));

        return 0;
    }

    /// <summary>
    /// 召喚一位「史萊姆 (Slime)」 角色。
    /// 此史萊姆角色將會加入行動角色所屬軍隊的尾端，在此回合中史萊姆已能開始行動。
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));

        // 史萊姆擁有 100 HP、0 MP、50 STR，沒有任何技能，並且初始為正常狀態。
        var slime = new Slime(GameIO)
        {
            Name = "Slime",
            HealthPoint = 100,
            MagicPoint = 0,
            Strength = 50,
            IsHero = false,
        };

        slime.DecisionMaker = new DefaultAiDecisionMaker();
        slime.EnterState(new NormalState(GameIO));

        if (executant.Troop == null)
        {
            throw new InvalidOperationException("Executant does not belong to any troop.");
        }

        game.AddRole(executant.Troop, slime);

        var summonRelation = new SummonRelation(executant, slime);

        slime.RegisterDeathObserver(summonRelation);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        // [2]Slime1 使用了 召喚。
        return $"{executant} 使用了 {Name}。";
    }
}
