using ShowDown.Enums;
using ShowDown.Models;

namespace Showdown.Test;

public class CardTests
{
    public static IEnumerable<object[]> CardComparisonData()
    {
        yield return new object[] { new Card(Suit.Clubs, Rank.Two), new Card(Suit.Diamonds, Rank.Two), -1 };
        yield return new object[] { new Card(Suit.Diamonds, Rank.Two), new Card(Suit.Hearts, Rank.Two), -1 };
        yield return new object[] { new Card(Suit.Hearts, Rank.Two), new Card(Suit.Spades, Rank.Two), -1 };
        yield return new object[] { new Card(Suit.Spades, Rank.Two), new Card(Suit.Clubs, Rank.Two), 1 };
    }

    [Theory]
    [MemberData(nameof(CardComparisonData))]
    public void GivenTheSameRank(Card card1, Card card2, int expected)
    {   
        var actual = card1.CompareTo(card2) switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GivenTheSameSuit()
    {
        // arrange
        var card1 = new Card(Suit.Clubs, Rank.Two);
        var card2 = new Card(Suit.Clubs, Rank.Three);
        
        // act
        var actual = card1.CompareTo(card2) switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
        
        // assert
        Assert.Equal(-1, actual);
    }

}