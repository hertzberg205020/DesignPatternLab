using RpgGame.Models.GameComponent.GameActions;

namespace RpgGame.Models.GameComponent.DecisionStrategies;

public interface IDecisionMaker
{
    GameAction SelectAction(List<GameAction> actions);
    List<Role> SelectTargets(List<Role> candidates, int requiredTargetCount);
}
