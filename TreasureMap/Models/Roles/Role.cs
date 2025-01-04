using TreasureMap.Models.MapComponents;
using TreasureMap.Models.States;

namespace TreasureMap.Models.Roles;

public abstract class Role : MapObject
{
    public State State { get; set; }

    public abstract int Hp { get; set; }

    public abstract int MaxHp { get; set; }

    public Role()
    {
        State = new Normal(this);
        State.Role = this;
    }

    public void EnterState(State state)
    {
        State.ExitState();
        State = state;
        // State.Role = this;
        State.EnterState();
    }

    public void Attack()
    {
        State.Attack();
    }

    public void Move()
    {
        State.Move();
    }

    public void TakeDamage(int damage)
    {
        State.TakeDamage(damage);
    }

    public abstract string TakeDecision(params string[] options);

    public virtual void Touch(MapObject mapObject)
    {
        mapObject.BeTouched(this);
    }

    public void OnTurnBegin()
    {
        State.OnTurnBegin();
    }

    public void OnTurnEnd()
    {
        State.OnTurnEnd();
    }

    public void ExecuteTurn()
    {
        Console.WriteLine($"{GetType()}目前的血量為 {Hp} 。");
        OnTurnBegin();
        State.DecreaseLeftRounds();
        Console.WriteLine($"{GetType()}目前的狀態為 {State.Name} 。");

        for (int i = 0; i < State.ActionCounts; i++)
        {
            State.BeforeAction();
            State.PerformAction();
            State.AfterAction();
        }

        OnTurnEnd();
    }

    public abstract Direction MoveDirection();

    public void Die()
    {
        GameMap?.RemoveObject(this);
    }
}
