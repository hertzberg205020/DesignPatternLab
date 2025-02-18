using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkill.OnePunchSkillHandler;

public class CheerupStateHandler : OnePunchHandler
{
    public CheerupStateHandler(OnePunchHandler? nextHandler, IGameIO gameIO)
        : base(nextHandler, gameIO) { }

    /// <summary>
    /// 目標角色的當前狀態為受到鼓舞狀態
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override bool IsMatch(Role target)
    {
        return target.State is CheerUpState;
    }

    /// <summary>
    /// 對目標角色造成 100 點傷害，並將目標角色的狀態恢復成正常狀態
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected override void Apply(Role executant, Role target)
    {
        executant.Attack(target, 100);

        if (target.IsAlive())
        {
            target.EnterState(new NormalState(GameIO));
        }
    }
}
