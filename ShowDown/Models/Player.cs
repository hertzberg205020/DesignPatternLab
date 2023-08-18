namespace ShowDown.Models;

public abstract class Player
{
    public IList<Card> HandCards { get; private set; } = new List<Card>();
}