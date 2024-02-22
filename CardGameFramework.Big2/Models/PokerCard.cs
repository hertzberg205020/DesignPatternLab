using System.Text.RegularExpressions;
using CardGameFramework.Big2.Enums;
using CardGameFramework.Big2.Helpers;
using CardGameFramework.Infra.Extensions;
using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public sealed class PokerCard: ICard, IComparable<PokerCard>
{
    public Rank Rank { get; }
    public Suit Suit { get; }

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
    
    // Parse a string to a PokerCard
    public static PokerCard Parse(string cardString)
    {
        // A[10] -> A, 10
        // determine the suit and rank of the card
        var match = Regex.Match(cardString, @"^([SHDC])\[(\d{1,2}|J|Q|K|A)\]$");
        if (!match.Success)
        {
            throw new ArgumentException("Invalid card string", nameof(cardString));
        }

        // 從匹配中提取花色和等級
        var suitSymbol = match.Groups[1].Value;
        var rankSymbol = match.Groups[2].Value;

        if (!CardHelper.SUIT_CACHE.TryGetValue(suitSymbol, out var suit) ||
            !CardHelper.RANK_CACHE.TryGetValue(rankSymbol, out var rank))
        {
            throw new ArgumentException("Invalid card string", nameof(cardString));
        }

        return new PokerCard(rank, suit);
    }

    public override bool Equals(object? obj)
    {
        if (obj is PokerCard other)
        {
            return Rank == other.Rank && Suit == other.Suit;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine((int)Rank, (int)Suit);
    }
}