using FluentAssertions;
using ShowDown.Enums;
using ShowDown.Models;
using Xunit.Abstractions;

namespace Showdown.Test;

public class CardTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CardTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

    [Fact]
    public void Given4AiPlayer_WhenOnPlayTurns()
    {
        // arrange
        var ai1 = new AiPlayer();
        var ai2 = new AiPlayer();
        var ai3 = new AiPlayer();
        var ai4 = new AiPlayer();
        var players = new List<Player> { ai1, ai2, ai3, ai4 };
        var deck = new Deck();
        var game = new ShowDownGame(players, deck);

        // act
        ai1.NameSelf("P1");
        ai2.NameSelf("P2");
        ai3.NameSelf("P3");
        ai4.NameSelf("P4");
        game.OnCardDraw();

        // assert
        ai1.ExchangeHandCards(ai2);
        ai2.ExchangeHandCards(ai3);
        
        
        
        ai1.HandCards.Count.Should().Be(13);
        ai2.HandCards.Count.Should().Be(13);
    }
    
    [Fact]
    public void Given4AiPlayers()
    {
        // arrange
        var ai1 = new AiPlayer();
        var ai2 = new AiPlayer();
        var ai3 = new AiPlayer();
        var ai4 = new AiPlayer();
        var players = new List<Player> { ai1, ai2, ai3, ai4 };
        var deck = new Deck();
        var game = new ShowDownGame(players, deck);

        // act
        game.OnCardDraw();

        // assert
        ai1.HandCards.Count.Should().Be(13);
        ai2.HandCards.Count.Should().Be(13);
        ai3.HandCards.Count.Should().Be(13);
        ai4.HandCards.Count.Should().Be(13);
    }
}