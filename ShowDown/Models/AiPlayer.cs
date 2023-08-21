namespace ShowDown.Models;

public class AiPlayer : Player
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
        if (HasExchangeHands) return;
        
        var shouldExchange = GetRandomBoolean();
        
        if (!shouldExchange) return;
        
        var player = GetRandomPlayer(players);
        ExchangeHandCards(player);
        HasExchangeHands = true;
    }

    private Player GetRandomPlayer(IList<Player> players)
    {
        var otherPlayers = players.Where(p => p != this).ToList();
        var index = _rnd.Next(0, otherPlayers.Count);
        return otherPlayers[index];
    }

    private bool GetRandomBoolean()
    {
        return _rnd.Next(2) == 0;
    }
}