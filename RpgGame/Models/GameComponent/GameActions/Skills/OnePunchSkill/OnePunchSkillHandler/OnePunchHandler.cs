using RpgGame.Models.GameComponent.IO;

namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkill.OnePunchSkillHandler;

public abstract class OnePunchHandler
{
    private readonly OnePunchHandler? _nextHandler;
    public readonly IGameIO GameIO;

    protected OnePunchHandler(OnePunchHandler? nextHandler, IGameIO gameIO)
    {
        _nextHandler = nextHandler;
        GameIO = gameIO;
    }

    public void Handle(Role executant, Role target)
    {
        if (IsMatch(target))
        {
            Apply(executant, target);
        }
        else
        {
            _nextHandler?.Handle(executant, target);
        }
    }

    protected abstract bool IsMatch(Role target);

    protected abstract void Apply(Role executant, Role target);
}
