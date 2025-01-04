using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Healing : State
{
    public Healing(Role role)
        : base(role, "恢復") { }

    public override int LeftRounds { get; set; } = 5;

    public override void OnTurnBegin()
    {
        HealRole(30); // Heal with a specified amount
    }

    private void HealRole(int healingAmount)
    {
        if (Role == null)
            return;

        var initialHp = Role.Hp;
        Role.Hp = Math.Min(Role.Hp + healingAmount, Role.MaxHp);

        if (initialHp == Role.MaxHp)
        {
            Console.WriteLine($"{Role.GetType()} 的生命值已達到最大值，無法再回復。");
            Role.EnterState(new Normal(Role));
            return;
        }

        Console.WriteLine($"回復 {Role.GetType()} {healingAmount} 點生命值。");
        Console.WriteLine($"目前 {Role.GetType()} 的生命值為 {Role.Hp}。");
    }
}
