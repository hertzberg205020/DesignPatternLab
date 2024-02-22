using System.Text;

namespace CardGameFramework.Infra.Models;

public abstract class CardPlayer<TCard> 
    where TCard : ICard
{
    public string? Name { get; set; }
    
    public CardGame<TCard>? CardGame { get; set; }

    public HandOfCards<TCard> HandOfCards { get; set; }
    
    protected CardPlayer()
    {
        HandOfCards = new HandOfCards<TCard>();
    }
    
    protected CardPlayer(HandOfCards<TCard> handOfCards)
    {
        HandOfCards = handOfCards;
    }
    
    public abstract void NameSelf(int order);
    
    public virtual void AddCardToHand(TCard card)
    {
        HandOfCards.Add(card);
    }
    
    public virtual void AddCardsToHand(IEnumerable<TCard> cards)
    {
        HandOfCards.AddRange(cards);
    }
    
    /// <summary>
    /// Play a card from hand
    /// </summary>
    /// <param name="index">the index of the card in hand</param>
    /// <returns>the card played</returns>
    public virtual TCard PlayCard(int index)
    {
        if (index < 0 || index >= HandOfCards.Cards.Count)
        {
            throw new IndexOutOfRangeException();
        }
        
        var selectedCard = HandOfCards.Cards[index];
        HandOfCards.Cards.RemoveAt(index);
        return selectedCard;
    }
    
    /// <summary>
    /// for testing purpose
    /// </summary>
    /// <param name="cards">specify the cards to be set to hand</param>
    public virtual void SetHand(IEnumerable<TCard> cards)
    {
        HandOfCards.Cards.Clear();
        HandOfCards.AddRange(cards);
    }
    
    public virtual void DisplayCardSelections()
    {
        Console.WriteLine($"{Name}'s cards on hand: ");
        Console.WriteLine(RevealCardsOnHand());
    }
    
    private string RevealCardsOnHand()
    {
        StringBuilder result = new StringBuilder();
        var cardCount = HandOfCards.Cards.Count;

        for (var i = 0; i < cardCount; i++)
        {
            var formattedIndex = (i).ToString("00");
            result.Append(formattedIndex).Append(") ");
            result.Append(HandOfCards.Cards[i]).Append(",\t");
        }

        return result.ToString();
    }
}