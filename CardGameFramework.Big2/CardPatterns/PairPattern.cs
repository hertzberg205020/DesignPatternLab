using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.CardPatterns;

public class PairPattern: ICardPattern, IComparable<PairPattern>
{
    public IList<PokerCard> Contents { get; set; }
    
    public PairPattern(IList<PokerCard> contents)
    {
        Contents = contents;
    }
    
    public PokerCard GetHighestCard()
    {
        return Contents.Max(card => card)!;
    }
    
    public static bool IsValid(IList<PokerCard> cards)
    {
        return cards.Count == 2 && cards[0].Rank == cards[1].Rank;
    }

    public int CompareTo(PairPattern? other)
    {
        if (other == null)
        {
            return 1;
        }
        return GetHighestCard().CompareTo(other.GetHighestCard());
    }
}