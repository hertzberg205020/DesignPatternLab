using CardGameFramework.Models.Commons;

namespace CardGameFramework.Models.UnoGame;

public class CardTable
{
    public UnoCard TopUnoCard { get; set; }
    
    private readonly List<UnoCard> _playedCards = new();

    public UnoCardGame Game { get; set; }

    public Deck<UnoCard> Deck => Game.Deck;
    
    public void RefillDeck()
    {
        Game.Deck.Refill(_playedCards);
    }
    
    public void ChangeTopCard(UnoCard card)
    {
        _playedCards.Add(TopUnoCard);
        TopUnoCard = card;
    }
}