using System.Text;

namespace CardGameFramework.Infra.Models;

public abstract class CardPlayer<TCard> 
    where TCard : ICard
{
    public string? Name { get; set; }
    
    public CardGame<TCard> CardGame { get; set; }

    public HandOfCards<TCard> HandOfCards { get; set; } = new HandOfCards<TCard>();

    public abstract void NameSelf(int order);
    
    public void AddCardToHand(TCard card)
    {
        HandOfCards.Add(card);
    }
    
    /// <summary>
    /// Play a card from hand
    /// </summary>
    /// <param name="index">the index of the card in hand</param>
    /// <returns>the card played</returns>
    public TCard PlayCard(int index)
    {
        if (index < 0 || index >= HandOfCards.Cards.Count)
        {
            throw new IndexOutOfRangeException();
        }
        
        var selectedCard = HandOfCards.Cards[index];
        HandOfCards.Cards.RemoveAt(index);
        return selectedCard;
    }
    
    public virtual void DisplayCardsSelections()
    {
        Console.WriteLine($"{Name}'s cards on hand: ");
        Console.WriteLine(RevealCardsOnHand());
    }
    
    public virtual string RevealCardsOnHand()
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