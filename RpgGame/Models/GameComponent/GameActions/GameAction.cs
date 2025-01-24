using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions;

public abstract class GameAction
{
    public int MpCost { get; set; }

    public readonly string Name;

    public TargetType TargetType { get; }

    protected GameAction(string name, int mpCost, TargetType targetType)
    {
        MpCost = mpCost;
        TargetType = targetType;
        Name = name;
    }

    public abstract int GetRequiredTargetCount(Game game, Role self);

    public abstract void DoExecute(Game game, Role executant, List<Role> targets);

    /// <summary>
    /// execute the action
    /// </summary>
    /// <param name="game">the RPG</param>
    /// <param name="executant">the role who executes the action</param>
    /// <param name="targets">the roles who are targeted</param>
    public virtual void Execute(Game game, Role executant, List<Role> targets)
    {
        Console.WriteLine(FormatExecuteMessage(executant, targets));
        DoExecute(game, executant, targets);
    }

    public abstract string FormatExecuteMessage(Role executant, List<Role> targets);

    public override string ToString()
    {
        return Name;
    }
}
