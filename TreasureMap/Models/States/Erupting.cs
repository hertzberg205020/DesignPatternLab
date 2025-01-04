using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Erupting : State
{
    public Erupting(Role role)
        : base(role, "爆發") { }

    public override int LeftRounds { get; set; } = 3;

    public override void Attack()
    {
        if (Role == null)
        {
            throw new InvalidOperationException("Role is null.");
        }

        var map = Role.GameMap;

        if (map == null)
        {
            throw new InvalidOperationException("GameMap is null.");
        }

        var targets = new List<Role>();

        switch (Role)
        {
            case Character:
                targets.AddRange(map.Monsters);
                break;
            case Monster:
            {
                if (map.Character != null)
                    targets.Add(map.Character);
                break;
            }
        }

        foreach (var monster in targets)
        {
            monster.TakeDamage(50);
        }
    }

    public override void NextState()
    {
        Role?.EnterState(new Teleport(Role));
    }
}
