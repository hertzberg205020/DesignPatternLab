using RpgGame.Models.GameComponent.DecisionStrategies;
using RpgGame.Models.GameComponent.GameActions.Skills;
using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent;

public class Slime : Role
{
    public Slime(IGameIO gameIO)
        : base(gameIO) { }
}
