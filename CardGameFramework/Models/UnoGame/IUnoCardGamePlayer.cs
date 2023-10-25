using CardGameFramework.Models.Commons;

namespace CardGameFramework.Models.UnoGame;

public interface IUnoCardGamePlayer
{
    CardTable CardTable { get; } 
    
    HandOfCards<UnoCard> HandOfCards { get; }
    
    string? Name { get; }
    
    void NameSelf(int order);

    TurnMove TakeTurn()
    {
        var card = LayCard();

        if (card == null)
        {
            return new TurnMove(this, null);
        }

        return new TurnMove(this, card);
    }
    
    UnoCard? LayCard();
    
    bool IsValidCardToLay(UnoCard selectedCard)
    {
        if (CardTable?.TopUnoCard is null)
        {
            return true;
        }
        return CardTable.TopUnoCard.MatchCard(selectedCard);
    }
    
    void AddCardToHand(UnoCard card);
    
}