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
            return $"Current magic points ({CurrentMagicPoints}) are not enough. Required: {TheRequiredMagicPoints}.";
        }
    }
}
