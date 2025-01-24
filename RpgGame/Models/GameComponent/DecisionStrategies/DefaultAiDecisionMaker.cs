using RpgGame.Models.GameComponent.GameActions;

namespace RpgGame.Models.GameComponent.DecisionStrategies;

/// <summary>
/// 每位 AI 將會存有一個 seed 屬性，初始值為 0，
/// 每位 AI 依賴自己的 seed 屬性來做各項決策，
/// 每次做完一項決策，seed 的值就會 + 1。
/// AI 需要從 n 個候選角色中選擇 m 個目標時,它會使用目前的 seed 值作為起始點,依序選擇目標。
/// 選擇的方式是:對於第 k 個目標(k 從 0 開始),會選擇索引為 (seed+k)%n 的候選角色。
/// </summary>
public class DefaultAiDecisionMaker : IDecisionMaker
{
    private int _seed = 0;

    public GameAction SelectAction(List<GameAction> actions)
    {
        var actionList = actions.ToList();
        var actionCount = actionList.Count;
        var selectedAction = actionList[_seed % actionCount];
        _seed++;
        return selectedAction;
    }

    public List<Role> SelectTargets(List<Role> candidates, int requiredTargetCount)
    {
        var targetCount = Math.Min(requiredTargetCount, candidates.Count);
        var selectedTargets = new List<Role>();
        for (var i = 0; i < targetCount; i++)
        {
            var targetIndex = (_seed + i) % candidates.Count;
            selectedTargets.Add(candidates[targetIndex]);
        }
        _seed++;
        return selectedTargets;
    }
}
