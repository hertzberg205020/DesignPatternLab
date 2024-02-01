using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.CardPatterns;

public class FullHousePattern: ICardPattern, IComparable<FullHousePattern>
{
    public IList<PokerCard> Contents { get; set; }
    
    public readonly IList<PokerCard> Triplet;
    
    public readonly IList<PokerCard> Pair;
    
    public readonly PokerCard HighestCardInTriplet;
    
    public FullHousePattern(IList<PokerCard> contents)
    {
        Contents = contents;
        var rankCounts = contents.GroupBy(card => card.Rank)
            .OrderBy(g => g.Count())
            .ToList();
        Pair = rankCounts[0].ToList();
        Triplet = rankCounts[1].ToList();
        HighestCardInTriplet = Triplet.Max(card => card)!;
    }
    
    public static bool IsValid(IList<PokerCard> cards)
    {
        if (cards.Count != 5)
        {
            return false;
        }
        var rankCounts = cards.GroupBy(card => card.Rank)
            .Select(group => group.Count())
            .ToList();
        rankCounts.Sort();
        return rankCounts.SequenceEqual(new List<int>() {2, 3});
    }

    public int CompareTo(FullHousePattern? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return HighestCardInTriplet.CompareTo(other.HighestCardInTriplet);
    }
}