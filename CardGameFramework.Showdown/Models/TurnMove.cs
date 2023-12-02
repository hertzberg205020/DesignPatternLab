using CardGameFramework.Showdown.Models;

namespace CardGameFramework.Models.ShowDownGame;

public class TurnMove
{
    public readonly IShowDownGamePlayer Player;
    
    public readonly PokerCard Card;

    public TurnMove(IShowDownGamePlayer player, PokerCard card)
    {
        Player = player;
        Card = card;
    }
}