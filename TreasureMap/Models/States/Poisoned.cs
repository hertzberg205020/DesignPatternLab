namespace TreasureMap.Models.States;

public class Poisoned : State
{
    public override int LeftRounds { get; set; } = 3;

    public override string Name { get; } = "中毒";

    public override void OnTurnBegin()
    {
        Console.WriteLine("中毒效果造成 15 點傷害。");

        if (Role == null)
        {
            throw new InvalidOperationException("Role is null.");
        }
        Role.Hp -= 15;
        Console.WriteLine($"造成 {Role.GetType()} 目前身命值為 ${Role.Hp} 點。");

        if (Role.Hp <= 0)
        {
            Role.Die();
        }
    }
}
