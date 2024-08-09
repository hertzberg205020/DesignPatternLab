namespace TreasureMap.Models.States;

public class Invincible : State
{
    public override int LeftRounds { get; set; } = 2;

    public override string Name { get; } = "無敵";

    public override void TakeDamage(int damage)
    {
        Console.WriteLine("無法造成傷害。");
    }
}
