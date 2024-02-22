using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.CardPatterns;

public class SinglePattern : ICardPattern, IComparable<SinglePattern>
{
    public IList<PokerCard> Contents { get; set; }

    public static string PatternName => "單張";
    
    public override string ToString() => string.Join(" ", Contents);
    
    public SinglePattern(IList<PokerCard> contents)
    {
        Contents = contents;
    }

    public static bool IsValid(IList<PokerCard> cards)
    {
        return cards.Count == 1;
    }

    public int CompareTo(SinglePattern? other)
    {
        if (other == null)
        {
            return 1;
        }
        return Contents[0].CompareTo(other.Contents[0]);
    }
}