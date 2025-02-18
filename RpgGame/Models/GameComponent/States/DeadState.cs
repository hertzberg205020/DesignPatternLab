using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.States;

public class DeadState : State
{
    public DeadState(IGameIO gameIO)
        : base("死亡", gameIO) { }

    public override void EnterState()
    {
        if (Role == null)
        {
            throw new InvalidOperationException("Role is null.");
        }

        Role.HealthPoint = 0;
        // Console.WriteLine($"{Role} 死亡。");
        GameIO.WriteLine($"{Role} 死亡。");
        Role.NotifyDeathObservers();
    }

    public override bool CanTakeAction { get; set; } = false;

    protected override void DecreaseLeftRounds()
    {
        // Do nothing
    }

    /// <summary>
    /// 死亡狀態不會造成傷害
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void Attack(Role target, int damage)
    {
        if (Role == null)
        {
            throw new InvalidOperationException("Role is null.");
        }

        ArgumentNullException.ThrowIfNull(target);

        if (damage < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(damage),
                "Damage must be greater than or equal to 0."
            );
        }
    }
}
