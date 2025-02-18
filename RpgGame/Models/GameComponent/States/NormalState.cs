using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.States;

public class NormalState : State
{
    public NormalState(IGameIO gameIO)
        : base("正常", gameIO) { }
}
