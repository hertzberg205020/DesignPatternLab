using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkill.OnePunchSkillHandler;

public class AbnormalStateHandler : OnePunchHandler
{
    public AbnormalStateHandler(OnePunchHandler? nextHandler, IGameIO gameIO)
        : base(nextHandler, gameIO) { }

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
    /// 對目標角色造成三次 80 點傷害。
    /// </summary>
    protected override void Apply(Role executant, Role target)
    {
        for (var i = 0; i < 3; i++)
        {
            if (target.IsDead())
            {
                break;
            }

            executant.Attack(target, 80);
        }
    }
}
