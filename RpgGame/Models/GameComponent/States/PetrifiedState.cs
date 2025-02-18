using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.States;

/// <summary>
/// 角色在此輪中無法行動，意即，此角色在此輪中的 S1、 S2 和 S3 將會被略過。
/// </summary>
public class PetrifiedState : State
{
    public PetrifiedState(IGameIO gameIO)
        : base("石化", gameIO) { }

    public override bool CanTakeAction { get; set; } = false;

    public override int LeftRounds { get; set; } = 3;

    public override void Attack(Role target, int damage)
    {
        throw new InvalidOperationException("cannot take action");
    }

    public override void AfterTakeAction()
    {
        DecreaseLeftRounds();
    }
}
