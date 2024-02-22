using CardGameFramework.Big2.CardBuilders;
using CardGameFramework.Big2.CardPatterns;
using CardGameFramework.Big2.Enums;
using CardGameFramework.Big2.Models;
using Xunit.Abstractions;

namespace CardGameFramework.Big2.Tests.Unit;

public class CardPatternTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public CardPatternTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetHighestCard_WithPairOfAces_ReturnsAceOfDiamonds()
    {
        // Arrange
        var cards = new List<PokerCard>()
        {
            new PokerCard(Rank.Ace, Suit.Club),
            new PokerCard(Rank.Ace, Suit.Diamond),
        };
        var pattern = new PairPattern(cards);
        // Act
        var highestCard = pattern.GetHighestCard();
        // Assert
        Assert.Equal(Suit.Diamond, highestCard.Suit);
    }
    
    [Fact]
    public void FullHouse()
    {
        // Arrange
        // C[4] S[4] C[K] D[K] S[K]
        var cards = new List<PokerCard>()
        {
            new PokerCard(Rank.Four, Suit.Club),
            new PokerCard(Rank.Four, Suit.Spade),
            new PokerCard(Rank.King, Suit.Club),
            new PokerCard(Rank.King, Suit.Diamond),
            new PokerCard(Rank.King, Suit.Spade),
        };
        // Act
        bool isValid = FullHousePattern.IsValid(cards);
        var pattern = CardPatternCreator.Instance.CreateCardPattern(cards);
        // var pattern = new FullHousePattern(cards);
        // Assert
        Assert.IsType<FullHousePattern>(pattern);
        // Assert.True(isValid);
    }
    
    [Fact]
    public void FullHouseCompare()
    {
        // Arrange
        // C[7] D[7] H[7] C[9] D[9]
        var cards1 = new List<PokerCard>()
        {
            new PokerCard(Rank.Seven, Suit.Club),
            new PokerCard(Rank.Seven, Suit.Diamond),
            new PokerCard(Rank.Seven, Suit.Heart),
            new PokerCard(Rank.Nine, Suit.Club),
            new PokerCard(Rank.Nine, Suit.Diamond),
        };
        // C[4] S[4] C[K] D[K] S[K]
        var cards2 = new List<PokerCard>()
        {
            new PokerCard(Rank.Four, Suit.Club),
            new PokerCard(Rank.Four, Suit.Spade),
            new PokerCard(Rank.King, Suit.Club),
            new PokerCard(Rank.King, Suit.Diamond),
            new PokerCard(Rank.King, Suit.Spade),
        };
        
        var pattern1 = (FullHousePattern)CardPatternCreator.Instance.CreateCardPattern(cards1);
        var pattern2 = (FullHousePattern)CardPatternCreator.Instance.CreateCardPattern(cards2);
        // Act
        var compareResult = pattern1.CompareTo(pattern2);
        
        _testOutputHelper.WriteLine(pattern1.HighestCardInTriplet.ToString());
        _testOutputHelper.WriteLine(pattern2.HighestCardInTriplet.ToString());
        // Assert
        Assert.True(compareResult < 0);
    }
}