namespace RpgGame.Models.GameComponent.States;

/// <summary>
/// 角色在此輪中無法行動，意即，此角色在此輪中的 S1、 S2 和 S3 將會被略過。
/// </summary>
public class PetrifiedState : State
{
    public PetrifiedState(Role role)
        : base("石化") { }

    public override bool CanTakeAction { get; set; } = false;

    public override int LeftRounds { get; set; } = 3;

    public override void AfterTakeAction()
    {
        DecreaseLeftRounds();
    }
}
