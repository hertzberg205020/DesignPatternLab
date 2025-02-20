﻿using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Stockpile : State
{
    public Stockpile(Role role)
        : base(role, "蓄力") { }

    public override int LeftRounds { get; set; } = 2;

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

    public override void NextState()
    {
        Role?.EnterState(new Erupting(Role));
    }
}
