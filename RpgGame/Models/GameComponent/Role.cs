using System.Text;
using RpgGame.Models.GameComponent.DecisionStrategies;
using RpgGame.Models.GameComponent.Exceptions;
using RpgGame.Models.GameComponent.GameActions;
using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;
using RpgGame.Models.Utils;

namespace RpgGame.Models.GameComponent;

public class Role
{
    public required string Name { get; set; }

    public required bool IsHero { get; set; }

    public required int HealthPoint { get; set; }

    public required int MagicPoint { get; set; }

    public required int Strength { get; set; }

    private readonly IGameIO _gameIO;

    /// <summary>
    /// 於每個角色最多只能處於一種狀態之下，每當角色獲得新的狀態時，就會覆寫舊有狀態，並且重新倒數三回合（含當前回合）。
    /// 三回合過後（含當前回合），角色的狀態會還原到正常狀態。
    /// when the role is initially created, the state is null
    /// </summary>
    public State? State { get; set; }

    public Game? Game { get; set; }

    public string? Troop { get; set; }

    public List<Skill> Skills { get; } = new();

    public IDecisionMaker? DecisionMaker;

    private readonly List<IRoleDeathObserver> _deathObservers = new();

    public Role(IGameIO gameIo)
    {
        _gameIO = gameIo;
    }

    public bool IsDead() => HealthPoint <= 0;

    public bool IsAlive() => !IsDead();

    /// <summary>
    /// 使用 IDecisionMaker 來決定角色的行動
    /// 1. 選擇一個行動
    /// 2. 選擇目標
    /// 3. 執行行動
    /// </summary>
    public void TakeTurn()
    {
        if (State == null)
        {
            throw new InvalidOperationException("The role is not in any state");
        }

        State.BeforeTakeAction();

        if (State.CanTakeAction)
        {
            TakeAction();
        }

        State.AfterTakeAction();
    }

    public void TakeAction()
    {
        var action = ChooseAction();
        var targets = ChooseTargets(action);
        PerformAction(action, targets);
    }

    public GameAction ChooseAction()
    {
        try
        {
            if (DecisionMaker == null)
            {
                throw new InvalidOperationException("DecisionMaker is not set");
            }

            var actions = new List<GameAction> { new BasicAttack(_gameIO) };

            actions.AddRange(Skills);

            var formattedActionList = GetFormattedActionList(actions);

            // Console.WriteLine(formattedActionList);
            _gameIO.WriteLine(formattedActionList);

            var action = DecisionMaker.SelectAction(actions);

            if (!ValidateMagicPoint(action))
            {
                throw new NotEnoughMagicPointException(MagicPoint, action.MpCost);
            }

            return action;
        }
        catch (NotEnoughMagicPointException e)
        {
            // Console.WriteLine(e);
            _gameIO.WriteLine(e.Message);
            return ChooseAction();
        }
    }

    /// <summary>
    /// return a formatted string of actions
    /// </summary>
    /// <example>
    /// 選擇行動：(0) 普通攻擊 (1) 鼓舞
    /// </example>
    /// <example>
    /// 選擇行動：(0) 普通攻擊 (1) 詛咒
    /// </example>
    /// <param name="actions">the list of actions</param>
    /// <returns></returns>
    private static string GetFormattedActionList(IEnumerable<GameAction> actions)
    {
        var sb = new StringBuilder();
        sb.Append("選擇行動：");
        var index = 0;
        foreach (var action in actions)
        {
            // (0) 普通攻擊
            sb.Append($"({index}) {action} ");
            index++;
        }
        return sb.ToString().TrimEnd();
    }

    private bool ValidateMagicPoint(GameAction action) => MagicPoint >= action.MpCost;

    public List<Role> ChooseTargets(GameAction action)
    {
        ArgumentNullException.ThrowIfNull(Game, nameof(Game));
        ArgumentNullException.ThrowIfNull(DecisionMaker, nameof(DecisionMaker));

        var requiredTargetCount = action.GetRequiredTargetCount(Game, this);
        var candidates = ObtainCandidates(action);

        if (requiredTargetCount == 0)
        {
            // return an empty list if no target is required
            return new List<Role>();
        }

        // Only show target selection prompt for human players
        if (candidates.Count <= requiredTargetCount)
        {
            return candidates;
        }

        return DecisionMaker.SelectTargets(candidates, requiredTargetCount);
    }

    public static string GenerateTargetList(List<Role> targets)
    {
        var sb = new StringBuilder();
        var index = 0;
        foreach (var target in targets)
        {
            // (0) [2]Slime1
            // (troopName) roleName
            sb.Append($"({index}) {target} ");
            index++;
        }

        return sb.ToString().TrimEnd();
    }

    public List<Role> ObtainCandidates(GameAction action)
    {
        var targetType = action.TargetType;

        if (targetType == TargetType.Self || targetType == TargetType.None)
        {
            return new List<Role>();
        }

        if (Game == null)
        {
            throw new InvalidOperationException("Rpg is not set");
        }

        var allies = Game.GetAllies(this).Where(r => r.IsAlive() && r != this).ToList();
        var enemies = Game.GetEnemies(this).Where(r => r.IsAlive()).ToList();

        return targetType switch
        {
            TargetType.Ally => allies,
            TargetType.Enemy => enemies,
            TargetType.All => allies.Concat(enemies).ToList(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// 扣除此行動所需的 MP
    /// 再執行此行動
    /// </summary>
    /// <param name="action"></param>
    /// <param name="targets"></param>
    public void PerformAction(GameAction action, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(Game, nameof(Game));

        action.Execute(Game, this, targets);
    }

    public void TakeDamage(int damage)
    {
        ValidationUtils.ShouldNotBeNegative(damage);

        HealthPoint = Math.Max(0, HealthPoint - damage);

        if (IsDead())
        {
            EnterState(new DeadState(_gameIO));
        }
    }

    public void Attack(Role target, int damage)
    {
        if (State == null)
        {
            throw new InvalidOperationException("The role is not in any state");
        }

        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (damage < 0)
        {
            throw new ArgumentException("Damage should not be negative");
        }

        State.Attack(target, damage);
    }

    public void EnterState(State state)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (State == state)
        {
            return;
        }

        if (State != null)
        {
            State.ExitState();
            State.Role = null;
        }

        State = state;
        State.Role = this;
        State.EnterState();
    }

    public void TakeHeal(int heal)
    {
        ValidationUtils.ShouldNotBeNegative(heal);

        HealthPoint += heal;
    }

    public void RegisterDeathObserver(IRoleDeathObserver observer)
    {
        _deathObservers.Add(observer);
    }

    public void NotifyDeathObservers()
    {
        foreach (var observer in _deathObservers)
        {
            observer.OnRoleDeath();
        }
    }

    /// <summary>
    /// Obtain the list of death observers
    /// </summary>
    /// <returns>an immutable list of death observers</returns>
    public IEnumerable<IRoleDeathObserver> GetDeathObservers()
    {
        return _deathObservers.AsReadOnly();
    }

    /// <summary>
    /// Format: [1]英雄
    /// </summary>
    public override string ToString()
    {
        if (Troop == null)
        {
            throw new InvalidOperationException("Troop is not set");
        }

        return $"[{Troop}]{Name}";
    }
}
