using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkill.OnePunchSkillHandler;

public class HpGreaterThanFiveHundredHandler : OnePunchHandler
{
    public HpGreaterThanFiveHundredHandler(OnePunchHandler? nextHandler, IGameIO gameIO)
        : base(nextHandler, gameIO) { }

    /// <summary>
    /// 目標角色的生命值 ≥ 500
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override bool IsMatch(Role target)
    {
        return target.HealthPoint >= 500;
    }

    /// <summary>
    /// 對目標角色造成 300 點傷害
    /// </summary>
    protected override void Apply(Role executant, Role target)
    {
        executant.Attack(target, 300);
    }
}
