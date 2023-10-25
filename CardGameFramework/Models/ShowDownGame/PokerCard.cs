using CardGameFramework.Enums;
using CardGameFramework.Extensions;
using CardGameFramework.Models.Commons;

namespace CardGameFramework.Models.ShowDownGame;

public class PokerCard: Card<Suit, Rank>, IComparable<PokerCard>
{
    public PokerCard(Suit category, Rank value) : base(category, value)
    {
    }

    public int CompareTo(PokerCard? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return -1;
        
        var rankComparison = Value.CompareTo(other.Value);
        return rankComparison != 0 ? rankComparison : Category.CompareTo(other.Category);
    }
    
    /// <summary>
    /// 使用領域知識的方法，撲克牌比較大小的方法
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int Showdown(PokerCard? other)
    {
        return CompareTo(other);
    }

    public override string ToString()
    {
        return $"{Category.GetDisplayName()}{Value.GetDisplayName()}";
    }
}