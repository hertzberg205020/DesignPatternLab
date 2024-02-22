using CardGameFramework.Big2.Enums;
using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.CardPatterns;

public class StraightPattern : ICardPattern, IComparable<StraightPattern>
{
    public IList<PokerCard> Contents { get; set; }

    public static string PatternName => "順子";

    public override string ToString() => string.Join(" ", Contents);

    public StraightPattern(IList<PokerCard> contents)
    {
        var orderCards = contents
            .GroupBy(p => p.Rank < Rank.Seven)
            .OrderByDescending(g => g.Key)
            .SelectMany(group => group.OrderBy(item => item))
            .ToList();
        Contents = orderCards;
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

        // 對卡牌進行排序
        var sortedCards = cards.OrderBy(card => card.Rank).ToList();
        
        // A既可在順的最後，也可在順的最前，但不能在順的中間
        // 順子 (Straight)： 任何連續數字的五張牌。
        // 大小比較規則：將五張牌中最大的牌作為比較基準；例如：2-3-4-5-6 > 10-J-Q-K-A
        
        var maxCard = sortedCards[4];
        var minCard = sortedCards[0];
        
        if (maxCard.Rank == Rank.Two && minCard.Rank == Rank.Three)
        {
            // "34567" 普通順子
            // "23456" 特殊順子
            // "A2345" 特殊順子
            // "KA234" 特殊順子
            // "QKA23" 特殊順子
            // "JQKA2" 普通順子
            return sortedCards
                .GroupBy(p => p.Rank < Rank.Seven)
                .Select(g => g.OrderBy(item => item).ToList())
                .All(IsValidStraight);
        }
        
        return IsValidStraight(sortedCards);
    }

    private static bool IsValidStraight(List<PokerCard> sortedCards)
    {
        for (int i = 0; i < sortedCards.Count - 1; i++)
        {
            if (sortedCards[i].Rank + 1 != sortedCards[i + 1].Rank)
            {
                return false;
            }
        }

        return true;
    }
    
    private static StraightPattern Initialize(IList<PokerCard> cards)
    {
        var cardPattern = cards
            .GroupBy(p => p.Rank < Rank.Seven)
            .OrderByDescending(g => g.Key)
            .SelectMany(group => group.OrderBy(item => item));
        return new StraightPattern(cardPattern.ToList());
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