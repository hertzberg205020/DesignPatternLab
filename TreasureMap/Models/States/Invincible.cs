using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Invincible : State
{
    public Invincible(Role role)
        : base(role, "無敵") { }

    public override int LeftRounds { get; set; } = 2;

    public override void TakeDamage(int damage)
    {
        Console.WriteLine("無法造成傷害。");
    }
}
