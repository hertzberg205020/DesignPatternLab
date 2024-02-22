using CardGameFramework.Big2.Models;
using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Tests.Unit.Mock;

public class MockDeck: IDeck<PokerCard>
{
    private readonly Stack<PokerCard> _cards = new ();
    
    public void Shuffle() { }

    public PokerCard Draw()
    {
        return _cards.Pop();
    }

    public bool IsEmpty()
    {
        return _cards.Count == 0;
    }
    
    public void Refill(List<PokerCard> cards)
    {
        cards.ForEach(card => _cards.Push(card));
    }
    
    public static IDeck<PokerCard> CreateDeck()
    {
        var deck = new MockDeck();
        var deckString = Console.ReadLine();
        var cards = deckString.Split(' ').Select(PokerCard.Parse).ToList();
        if (cards.Count != 52)
        {
            throw new ArgumentException("Invalid deck string");
        }
        deck.Refill(cards);
        return deck;
    }
}