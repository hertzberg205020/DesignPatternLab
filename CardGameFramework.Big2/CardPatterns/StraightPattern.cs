using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.CardPatterns;

public class StraightPattern : ICardPattern, IComparable<StraightPattern>
{
    public IList<PokerCard> Contents { get; set; }

    public StraightPattern(IList<PokerCard> contents)
    {
        Contents = contents;
    }
    
    public PokerCard GetHighestCard()
    {
        return Contents.Max(card => card)!;
    }
    
    public static bool IsValid(IList<PokerCard> cards)
    {
        if (cards.Count != 5)
        {
            return false;
        }
        var sortedCards = cards.OrderBy(card => card.Rank).ToList();
        for (int i = 0; i < cards.Count-1; i++)
        {
            if (cards[i].Rank + 1 != cards[i + 1].Rank)
            {
                return false;
            }
        }
        return true;
    }

    public int CompareTo(StraightPattern? other)
    {
        if (other == null)
        {
            return 1;
        }
        return GetHighestCard().CompareTo(other.GetHighestCard());
    }
}