using RpgGame.Models.GameComponent.States;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class Summon : Skill
{
    public Summon()
        : base("召喚", 150, TargetType.None) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 0;

    /// <summary>
    /// 召喚一位「史萊姆 (Slime)」 角色。
    /// 此史萊姆角色將會加入行動角色所屬軍隊的尾端，在此回合中史萊姆已能開始行動。
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));

        var slime = new Slime()
        {
            Name = "Slime",
            HealthPoint = 100,
            MagicPoint = 0,
            Strength = 10,
            State = new NormalState(),
            IsHero = false,
            Rpg = game,
            Troop = executant.Troop
        };

        ArgumentNullException.ThrowIfNull(executant.Troop, nameof(executant.Troop));

        game.AddRole(executant.Troop, slime);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        // [2]Slime1 使用了 召喚。
        return $"{executant.Name} 使用了 {Name}。";
    }
}
