using ShowDown.Enums;

namespace ShowDown.Models;

public class Card: IComparable<Card>
{
    public readonly Suit Suit;
    
    public readonly Rank Rank;
    
    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    /// <summary>
    /// 當系統中的遊戲只有Showdow，撲克牌比較大小的方法
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Card? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var suitComparison = Suit.CompareTo(other.Suit);
        if (suitComparison != 0) return suitComparison;
        return Rank.CompareTo(other.Rank);
    }
}