using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Accelerated : State
{
    public Accelerated(Role role)
        : base(role, "加速") { }

    public override int LeftRounds { get; set; } = 3;

    public override int ActionCounts { get; set; } = 2;

    public override void TakeDamage(int damage)
    {
        if (Role == null)
        {
            throw new InvalidOperationException("Role is null.");
        }

        Role.Hp -= damage;

        if (Role.Hp <= 0)
        {
            Role?.Die();
        }

        Role?.EnterState(new Normal(Role));
    }
}
