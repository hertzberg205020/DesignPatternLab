namespace CardGameFramework.Big2.Models;

public class TurnMove
{
    public readonly IBig2GamePlayer Player;
    
    public readonly PokerCard Card;

    public TurnMove(IBig2GamePlayer player, PokerCard card)
    {
        Player = player;
        Card = card;
    }
}