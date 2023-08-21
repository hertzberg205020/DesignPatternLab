namespace ShowDown.Models;

public class AiPlayer: Player
{
    private readonly Random _rnd = new();
    
    
    public override Card Show()
    {
        var index = _rnd.Next(0, HandCards.Count);
        var card = HandCards[index];
        HandCards.RemoveAt(index);
        return card;
    }

    public override void MakeExchangeDecision(IList<Player> players)
    {
        // TODO
    }
}