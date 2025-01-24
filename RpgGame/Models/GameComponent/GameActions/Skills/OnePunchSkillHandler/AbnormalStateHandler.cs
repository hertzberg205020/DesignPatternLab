using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkillHandler;

public class AbnormalStateHandler : OnePunchHandler
{
    public AbnormalStateHandler(OnePunchHandler? nextHandler)
        : base(nextHandler) { }

    /// <summary>
    /// 目標角色的當前狀態為中毒狀態或是石化狀態
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override bool IsMatch(Role target)
    {
        return target.State is PoisonedState or PetrifiedState;
    }

    /// <summary>
    /// 對目標角色造成 300 點傷害
    /// </summary>
    protected override void Apply(Role executant, Role target)
    {
        executant.Attack(target, 300);
    }
}
