using TreasureMap.Models.MapComponents;
using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public abstract class State
{
    protected State(Role role, string name)
    {
        Role = role;
        Name = name;
    }

    public virtual int LeftRounds { get; set; } = 0;

    public virtual int ActionCounts { get; set; } = 1;

    public string Name { get; }

    public Role Role { get; set; }

    public virtual void EnterState() { }

    public virtual void ExitState() { }

    public void DecreaseLeftRounds()
    {
        if (this is Normal)
        {
            return;
        }

        if (LeftRounds > 0)
        {
            LeftRounds--;
        }
        else
        {
            NextState();
        }
    }

    public virtual void NextState()
    {
        Role?.EnterState(new Normal(Role));
    }

    public virtual void TakeDamage(int damage)
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

        if (Role is Character && !(this is Accelerated || this is Stockpile))
        {
            Console.WriteLine($"造成主角身命值減少 {damage} 點。");
            Role.EnterState(new Invincible(Role));
        }
    }

    public virtual void Attack()
    {
        switch (Role)
        {
            case Character character:
            {
                // 對每個怪物進行攻擊
                var map = character.GameMap;
                var targets = map?.FindDestructibleMonsters(character);

                if (targets is not { Count: > 0 })
                    return;

                foreach (var target in targets)
                {
                    target.TakeDamage(10);
                }

                break;
            }
            case Monster monster:
            {
                // 對主角進行攻擊
                var target = Role.GameMap?.Character;
                target?.TakeDamage(50);
                Console.WriteLine("怪物對主角造成 50 點傷害。");
                break;
            }
            case null:
                throw new InvalidOperationException("Role is null.");
        }
    }

    public virtual void OnTurnBegin() { }

    public virtual void OnTurnEnd() { }

    public virtual void PerformAction()
    {
        var res = Role?.TakeDecision(new[] { "Attack", "Move" });
        Console.WriteLine(res);
        if (res == "Attack")
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    public virtual void Move()
    {
        if (Role == null)
            throw new InvalidOperationException("Role is null.");

        Coordinate? newCoordination = null;

        var direction = Role.MoveDirection();

        if (Role is Character character)
            character.Direction = direction;

        newCoordination = Role.Position?.GetNextCoordinatesBasedOnDirection(direction);

        if (newCoordination is null)
            throw new InvalidOperationException("New coordination is null.");

        var map = Role.GameMap;

        if (map == null)
            throw new InvalidOperationException("GameMap is null.");

        var res = map.MoveRole(Role, newCoordination);
        if (!res)
        {
            Console.WriteLine("無法移動到該位置。");
        }
    }

    public virtual void BeforeAction() { }

    public virtual void AfterAction() { }
}
