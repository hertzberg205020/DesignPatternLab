using CardGameFramework.Big2.Enums;
using CardGameFramework.Infra.Extensions;
using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public sealed class PokerCard: ICard, IComparable<PokerCard>
{
    public readonly Rank Rank;
    public readonly Suit Suit;

    public PokerCard(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    /// <summary>
    /// Compare two poker cards by their rank.
    /// if the rank is the same, compare their suit.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int CompareTo(PokerCard? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (Rank == other.Rank)
        {
            return Suit.CompareTo(other.Suit);
        }

        return Rank.CompareTo(other.Rank);
    }
    
    public bool IsSameRank(PokerCard otherCard)
    {
        return this.Rank == otherCard.Rank;
    }

    public override string ToString()
    {
        return $"{Suit.GetDisplayName()}[{Rank.GetDisplayName()}]";
    }
}