using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.States;

/// <summary>
/// 輪到此角色時，此角色在進入 S1 階段前的 HP 會減少 30，
/// 如果 HP 減少後角色 HP 歸零，則此角色死亡且無法行動。
/// </summary>
public class PoisonedState : State
{
    public PoisonedState(IGameIO gameIO)
        : base("中毒", gameIO) { }

    public override int LeftRounds { get; set; } = 3;

    public override void BeforeTakeAction()
    {
        if (Role == null)
        {
            throw new InvalidOperationException("The state is not attached to any role");
        }

        Role.TakeDamage(30);
    }

    public override void AfterTakeAction()
    {
        DecreaseLeftRounds();
    }
}
