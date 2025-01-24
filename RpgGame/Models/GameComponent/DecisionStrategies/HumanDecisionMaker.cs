using RpgGame.Models.GameComponent.GameActions;

namespace RpgGame.Models.GameComponent.DecisionStrategies;

public class HumanDecisionMaker : IDecisionMaker
{
    /// <summary>
    /// use standard input to get the action
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public GameAction SelectAction(List<GameAction> actions)
    {
        ArgumentNullException.ThrowIfNull(actions, nameof(actions));
        var index = GetValidIndex(actions.Count);
        return actions[index - 1];
    }

    public List<Role> SelectTargets(List<Role> candidates, int requiredTargetCount)
    {
        ArgumentNullException.ThrowIfNull(candidates, nameof(candidates));
        ArgumentNullException.ThrowIfNull(requiredTargetCount, nameof(requiredTargetCount));

        var selectedTargets = new List<Role>();
        var targetCount = Math.Min(requiredTargetCount, candidates.Count);

        for (var i = 0; i < targetCount; i++)
        {
            var index = GetValidIndex(candidates.Count);
            selectedTargets.Add(candidates[index - 1]);
        }

        return selectedTargets;
    }

    /// <summary>
    /// zero-based index
    /// </summary>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    private int GetValidIndex(int maxCount)
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out var index) && index >= 0 && index < maxCount)
            {
                return index;
            }
            Console.WriteLine("Invalid input, please try again.");
        }
    }
}
