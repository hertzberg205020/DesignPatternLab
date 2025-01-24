using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkillHandler;

public class NormalStateHandler : OnePunchHandler
{
    public NormalStateHandler(OnePunchHandler? nextHandler)
        : base(nextHandler) { }

    /// <summary>
    /// 目標角色的當前狀態為正常狀態
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected override bool IsMatch(Role target)
    {
        return target.State is NormalState;
    }

    /// <summary>
    /// 對目標角色造成 100 點傷害
    /// </summary>
    protected override void Apply(Role executant, Role target)
    {
        executant.Attack(target, 100);
    }
}
