using System.Text;
using RpgGame.Models.GameComponent.DecisionStrategies;
using RpgGame.Models.GameComponent.Exceptions;
using RpgGame.Models.GameComponent.GameActions;
using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameComponent.States;
using RpgGame.Models.GameLogic;
using RpgGame.Models.Utils;

namespace RpgGame.Models.GameComponent;

public class Role
{
    public required string Name { get; set; }

    public required bool IsHero { get; set; }

    public required int HealthPoint { get; set; }

    public required int MagicPoint { get; set; }

    public required int Strength { get; set; }

    /// <summary>
    /// 於每個角色最多只能處於一種狀態之下，每當角色獲得新的狀態時，就會覆寫舊有狀態，並且重新倒數三回合（含當前回合）。
    /// 三回合過後（含當前回合），角色的狀態會還原到正常狀態。
    /// </summary>
    private State _state;

    public State State
    {
        get => _state;
        set
        {
            _state = value;
            _state.Role = this;
        }
    }

    public Game? Rpg { get; set; }

    public string? Troop { get; set; }

    public IEnumerable<Skill>? Skills { get; set; }

    public IDecisionMaker? DecisionMaker;

    private readonly List<IRoleDeathObserver> _deathObservers = new();

    public Role() { }

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
        State.BeforeTakeAction();

        if (IsDead())
        {
            return;
        }

        if (State.CanTakeAction)
        {
            var action = ChooseAction();
            var target = ChooseTargets(action);
            PerformAction(action, target);
        }

        State.AfterTakeAction();
    }

    public GameAction ChooseAction()
    {
        try
        {
            var actions = new List<GameAction> { new BasicAttack() };
            ArgumentNullException.ThrowIfNull(DecisionMaker, nameof(DecisionMaker));

            ArgumentNullException.ThrowIfNull(Skills, nameof(Skills));

            actions.AddRange(Skills);

            var formattedActionList = GetFormattedActionList(actions);
            Console.WriteLine(formattedActionList);

            var action = DecisionMaker.SelectAction(actions);

            if (ValidateMagicPoint(action))
            {
                throw new NotEnoughMagicPointException(MagicPoint, action.MpCost);
            }

            return action;
        }
        catch (NotEnoughMagicPointException e)
        {
            Console.WriteLine(e);
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
        sb.AppendLine("選擇行動：");
        var index = 0;
        foreach (var action in actions)
        {
            // (0) 普通攻擊
            sb.AppendLine($"({index}) {action} ");
            index++;
        }
        // trim the last space and use a new line
        sb.Length -= 1;
        sb.AppendLine();

        return sb.ToString();
    }

    private bool ValidateMagicPoint(GameAction action) => MagicPoint < action.MpCost;

    public List<Role> ChooseTargets(GameAction action)
    {
        ArgumentNullException.ThrowIfNull(Rpg, nameof(Rpg));
        ArgumentNullException.ThrowIfNull(DecisionMaker, nameof(DecisionMaker));

        var requiredTargetCount = action.GetRequiredTargetCount(Rpg, this);

        var candidates = ObtainCandidates(action);

        if (requiredTargetCount == 0 || candidates.Count <= requiredTargetCount)
        {
            return candidates;
        }
        // 選擇 1 位目標: (0) [2]Slime1 (1) [2]Slime2
        Console.WriteLine($"選擇 {requiredTargetCount} 位目標: {GenerateTargetList(candidates)}");
        return DecisionMaker.SelectTargets(candidates, requiredTargetCount);
    }

    public static string GenerateTargetList(List<Role> targets)
    {
        var sb = new StringBuilder();
        var index = 0;
        foreach (var target in targets)
        {
            // (0) [2]Slime1
            sb.AppendLine($"({index}) {target} ");
            index++;
        }

        return sb.ToString().Trim();
    }

    private List<Role> ObtainCandidates(GameAction action)
    {
        var targetType = action.TargetType;

        if (targetType == TargetType.Self || targetType == TargetType.None)
        {
            return new List<Role>();
        }

        ArgumentNullException.ThrowIfNull(Rpg, nameof(Rpg));

        var allies = Rpg.GetAllies(this).Where(r => r.IsAlive() && r != this).ToList();
        var enemies = Rpg.GetEnemies(this).Where(r => r.IsAlive()).ToList();

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
        ArgumentNullException.ThrowIfNull(Rpg, nameof(Rpg));

        MagicPoint -= action.MpCost;
        action.Execute(Rpg, this, targets);
    }

    public void TakeDamage(int damage)
    {
        ValidationUtils.ShouldNotBeNegative(nameof(damage), damage);

        HealthPoint = Math.Max(0, HealthPoint - damage);

        if (IsDead())
        {
            EnterState(new DeadState(this));
        }
    }

    public void Attack(Role target, int damage)
    {
        State.Attack(target, damage);
    }

    public void EnterState(State state)
    {
        State.ExitState();
        State = state;
        State.EnterState();
    }

    public void TakeHeal(int heal)
    {
        ValidationUtils.ShouldNotBeNegative(nameof(heal), heal);

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
}
