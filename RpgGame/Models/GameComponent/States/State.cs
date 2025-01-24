namespace RpgGame.Models.GameComponent.States;

/// <summary>
/// 於每個角色最多只能處於一種狀態之下，每當角色獲得新的狀態時，就會覆寫舊有狀態，並且重新倒數三回合（含當前回合）。
/// 三回合過後（含當前回合），角色的狀態會還原到正常狀態。
/// </summary>
public class State
{
    public Role? Role;

    public string Name { get; init; }

    public virtual bool CanTakeAction { get; set; } = true;

    public virtual int LeftRounds { get; set; } = 0;

    public virtual int AttackDamage =>
        Role?.Strength ?? throw new ArgumentException("Role is null");

    protected State(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// the hook method for entering the state
    /// </summary>
    public virtual void EnterState() { }

    /// <summary>
    /// the hook method for exiting the state
    /// </summary>
    public virtual void ExitState() { }

    /// <summary>
    /// if the state has left rounds, decrease it by 1
    /// </summary>
    protected virtual void DecreaseLeftRounds()
    {
        if (LeftRounds > 0)
        {
            LeftRounds--;
        }

        if (LeftRounds == 0)
        {
            NextState(new NormalState() { Role = this.Role });
        }
    }

    protected virtual void NextState(State nextState)
    {
        if (LeftRounds == 0)
        {
            Role?.EnterState(nextState);
        }
    }

    /// <summary>
    /// the hook method for before taking action
    /// </summary>
    public virtual void BeforeTakeAction() { }

    /// <summary>
    /// the hook method for after taking action
    /// </summary>
    public virtual void AfterTakeAction() { }

    public virtual void Attack(Role target, int damage)
    {
        // 輸出攻擊訊息
        ArgumentNullException.ThrowIfNull(Role, nameof(Role));

        Console.WriteLine(FormatAttackMessage(target, damage));
        target.TakeDamage(damage);
    }

    /// <summary>
    /// format the attack message
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    /// <returns></returns>
    protected virtual string FormatAttackMessage(Role target, int damage)
    {
        ArgumentNullException.ThrowIfNull(Role, nameof(Role));

        // [1]英雄 對 [2]Slime1 造成 50 點傷害。
        // [1]英雄 對 [2]Slime1 造成 100 點傷害。
        return $"{Role.Name} 對 {target.Name} 造成 {damage} 點傷害。";
    }
}
