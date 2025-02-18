namespace RpgGame.Models.GameComponent.Exceptions;

public class NotEnoughMagicPointException : ArgumentException
{
    public int CurrentMagicPoints { get; }

    public int TheRequiredMagicPoints { get; }

    public NotEnoughMagicPointException(int currentMagicPoints, int theRequiredMagicPoints)
    {
        CurrentMagicPoints = currentMagicPoints;
        TheRequiredMagicPoints = theRequiredMagicPoints;
    }

    public override string Message
    {
        get
        {
            return $"你缺乏 MP，不能進行此行動。";
        }
    }
}
