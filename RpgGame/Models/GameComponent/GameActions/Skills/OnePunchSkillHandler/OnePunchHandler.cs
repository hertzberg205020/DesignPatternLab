namespace RpgGame.Models.GameComponent.GameActions.Skills.OnePunchSkillHandler;

public abstract class OnePunchHandler
{
    private readonly OnePunchHandler? _nextHandler;

    protected OnePunchHandler(OnePunchHandler? nextHandler)
    {
        _nextHandler = nextHandler;
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
