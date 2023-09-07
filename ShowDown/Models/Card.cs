using ShowDown.Enums;
using ShowDown.Extensions;

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
        
        var rankComparison = Rank.CompareTo(other.Rank);
        return rankComparison != 0 ? rankComparison : Suit.CompareTo(other.Suit);
    }
    
    /// <summary>
    /// 使用領域知識的方法，撲克牌比較大小的方法
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int Showdown(Card? other)
    {
        return CompareTo(other);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Card card)
        {
            return false;
        }

        return Suit == card.Suit && Rank == card.Rank;
    }

    public override string ToString()
    {
        return $"{Suit.GetDisplayName()}{Rank.GetDisplayName()}";
    }
}