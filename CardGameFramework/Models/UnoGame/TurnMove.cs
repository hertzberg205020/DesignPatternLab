namespace CardGameFramework.Models.UnoGame;

public class TurnMove
{
    public readonly IUnoCardGamePlayer Player;
    
    public readonly UnoCard? Card;
    
    public bool IsPassing => Card == null;
    
    public bool IsContinue => Player.HandOfCards.Cards.Count > 0;

    public TurnMove(IUnoCardGamePlayer player, UnoCard? card)
    {
        Player = player;
        Card = card;
    }
}