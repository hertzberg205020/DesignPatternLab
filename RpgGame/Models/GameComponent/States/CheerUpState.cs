namespace RpgGame.Models.GameComponent.States;

/// <summary>
/// 此角色在執行每一個能造成傷害的行動中，
/// 對於每一位被傷害者都額外創造 50 滴 HP 的加成傷害。
/// </summary>
public class CheerUpState : State
{
    public CheerUpState(Role role)
        : base("受到鼓舞") { }

    public override int LeftRounds { get; set; } = 3;

    public override void Attack(Role target, int damage)
    {
        var damageCorrection = damage + 50;

        Console.WriteLine(FormatAttackMessage(target, damageCorrection));

        target.TakeDamage(damage + 50);
    }

    public override void BeforeTakeAction()
    {
        DecreaseLeftRounds();
    }
}
