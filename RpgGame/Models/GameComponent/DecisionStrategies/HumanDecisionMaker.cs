using System.Text;
using RpgGame.Models.GameComponent.GameActions;
using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.DecisionStrategies;

public class HumanDecisionMaker : IDecisionMaker
{
    private readonly IGameIO _gameIO;

    public HumanDecisionMaker(IGameIO gameIo)
    {
        _gameIO = gameIo;
    }

    /// <summary>
    /// use standard input to get the action
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public GameAction SelectAction(List<GameAction> actions)
    {
        ArgumentNullException.ThrowIfNull(actions, nameof(actions));
        if (actions.Count == 0)
        {
            throw new ArgumentException("actions cannot be empty.", nameof(actions));
        }

        var index = GetValidIndex(actions.Count);
        return actions[index];
    }

    public List<Role> SelectTargets(List<Role> candidates, int requiredTargetCount)
    {
        ArgumentNullException.ThrowIfNull(candidates, nameof(candidates));

        if (candidates.Count == 0)
        {
            throw new ArgumentException("candidates cannot be empty.", nameof(candidates));
        }

        ArgumentNullException.ThrowIfNull(requiredTargetCount, nameof(requiredTargetCount));

        if (requiredTargetCount <= 0)
        {
            throw new ArgumentException(
                "requiredTargetCount must be greater than 0.",
                nameof(requiredTargetCount)
            );
        }

        var targetCount = Math.Min(requiredTargetCount, candidates.Count);

        // Console.Write($"選擇 {targetCount} 位目標: ");
        // Console.WriteLine(GenerateTargetList(candidates));
        _gameIO.WriteLine($"選擇 {targetCount} 位目標: {GenerateTargetList(candidates)}");
        // parse the input to get the selected targets

        var selectedIndices = ParseInput(targetCount, candidates.Count);

        return selectedIndices.Select(index => candidates[index]).ToList();
    }

    private static string GenerateTargetList(List<Role> targets)
    {
        var sb = new StringBuilder();
        var index = 0;
        foreach (var target in targets)
        {
            // (0) [2]Slime1
            // (option index) target
            sb.Append($"({index}) {target} ");
            index++;
        }

        return sb.ToString().TrimEnd();
    }

    private List<int> ParseInput(int targetCount, int maxIndex)
    {
        var selectedIndices = new List<int>();

        while (selectedIndices.Count < targetCount)
        {
            var input = _gameIO.ReadLine();

            if (input == null)
            {
                throw new InvalidOperationException("Cannot read input.");
            }

            var indices = input.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            if (indices.Length != targetCount)
            {
                continue;
            }

            foreach (var index in indices)
            {
                if (int.TryParse(index, out var i) && i >= 0 && i < maxIndex)
                {
                    selectedIndices.Add(i);
                }
                else
                {
                    selectedIndices.Clear();
                    break;
                }
            }
        }

        return selectedIndices;
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
            var input = _gameIO.ReadLine();

            // 無法解析就直接拋出例外
            if (input == null)
            {
                continue;
            }

            if (int.TryParse(input, out var index) && index >= 0 && index < maxCount)
            {
                return index;
            }
        }
    }
}
