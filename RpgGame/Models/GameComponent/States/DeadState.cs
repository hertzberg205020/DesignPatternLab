namespace RpgGame.Models.GameComponent.States;

public class DeadState : State
{
    public DeadState(Role role)
        : base("死亡") { }

    public override void EnterState()
    {
        if (Role == null)
        {
            throw new InvalidOperationException("Role is null.");
        }

        Role.HealthPoint = 0;

        // [1]A 死亡。
        Console.WriteLine($"{Role.Name} 死亡。");

        Role.NotifyDeathObservers();
    }
}
