using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.GameActions;

public abstract class GameAction
{
    public int MpCost { get; set; }

    public readonly string Name;

    public TargetType TargetType { get; }

    protected readonly IGameIO GameIO;

    protected GameAction(string name, int mpCost, TargetType targetType, IGameIO gameIO)
    {
        MpCost = mpCost;
        TargetType = targetType;
        GameIO = gameIO;
        Name = name;
        GameIO = gameIO;
    }

    public abstract int GetRequiredTargetCount(Game game, Role self);

    public abstract void Apply(Game game, Role executant, List<Role> targets);

    /// <summary>
    /// execute the action
    /// </summary>
    /// <param name="game">the RPG</param>
    /// <param name="executant">the role who executes the action</param>
    /// <param name="targets">the roles who are targeted</param>
    public virtual void Execute(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(executant);
        ArgumentNullException.ThrowIfNull(targets);

        ValidateMp(executant);

        ValidateTargetCount(game, executant, targets);

        ValidateTargetType(executant, game, targets);

        executant.MagicPoint -= MpCost;

        // Console.WriteLine(FormatExecuteMessage(executant, targets));
        GameIO.WriteLine(FormatExecuteMessage(executant, targets));
        Apply(game, executant, targets);
    }

    private void ValidateMp(Role executant)
    {
        if (executant.MagicPoint < MpCost)
        {
            throw new InvalidOperationException("魔力不足");
        }
    }

    private void ValidateTargetCount(Game game, Role executant, List<Role> targets)
    {
        if (targets.Count > GetRequiredTargetCount(game, executant))
        {
            throw new InvalidOperationException(
                $"目標數量不正確。輸入集合的元素數量: {targets.Count}，需要執行的元素數量: {GetRequiredTargetCount(game, executant)}"
            );
        }
    }

    private void ValidateTargetType(Role executant, Game game, List<Role> targets)
    {
        switch (TargetType)
        {
            case TargetType.Enemy when targets.Any(target => target.Troop == executant.Troop):
                throw new InvalidOperationException("無法對友軍使用");

            case TargetType.Ally when targets.Any(target => target.Troop != executant.Troop):
                throw new InvalidOperationException("無法對敵軍使用");

            case TargetType.Self
            or TargetType.None when targets.Count != 0:
                throw new InvalidOperationException("目標不正確");

            case TargetType.All when targets.Count != game.GetRoles().Count - 1:
                break;
        }
    }

    public abstract string FormatExecuteMessage(Role executant, List<Role> targets);

    public override string ToString()
    {
        return Name;
    }
}
