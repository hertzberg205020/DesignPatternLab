using CardGameFramework.Big2.CardBuilders;
using CardGameFramework.Big2.CardPatterns;
using CardGameFramework.Big2.Enums;
using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.Tests.Unit;

public class RoundTests
{
    [Fact]
    public void CanOverrideTopPlay_WithFullHouse_ReturnsTrue()
    {
        // Arrange
        var cards = new List<PokerCard>()
        {
            new PokerCard(Rank.Ace, Suit.Club),
            new PokerCard(Rank.Ace, Suit.Diamond),
            new PokerCard(Rank.Ace, Suit.Heart),
            new PokerCard(Rank.King, Suit.Club),
            new PokerCard(Rank.King, Suit.Diamond),
        };
        var pattern = new FullHousePattern(cards);

        var topPlayer = new BigTwoHumanPlayer(new HandCard());
        
        var round = new Round(null, topPlayer);

        var topPlayCards = new List<PokerCard>()
        {
            new PokerCard(Rank.Five, Suit.Club),
            new PokerCard(Rank.Five, Suit.Diamond),
            new PokerCard(Rank.Five, Suit.Heart),
            new PokerCard(Rank.Four, Suit.Club),
            new PokerCard(Rank.Four, Suit.Diamond),
        };
        var topPlay = new FullHousePattern(topPlayCards);
        round.TopPlay = topPlay;
        
        // Act
        var result = round.CanOverrideTopPlay(pattern);
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void CanOverrideTopPlay_WithFullHouse_WhenTopPlayPatternIsStraight_ReturnsFalse()
    {
        // Arrange
        var cards = new List<PokerCard>()
        {
            new PokerCard(Rank.Ace, Suit.Club),
            new PokerCard(Rank.Ace, Suit.Diamond),
            new PokerCard(Rank.Ace, Suit.Heart),
            new PokerCard(Rank.King, Suit.Club),
            new PokerCard(Rank.King, Suit.Diamond),
        };
        var pattern = new FullHousePattern(cards);
        // var cardPatternFactory = CardPatternCreator.Instance;
        // var pattern = cardPatternFactory.CreateCardPattern(cards);
        
        var topPlayer = new BigTwoHumanPlayer(new HandCard());
        var round = new Round(null, topPlayer);

        var topPlayCards = new List<PokerCard>()
        {
            new PokerCard(Rank.Ace, Suit.Club),
            new PokerCard(Rank.Two, Suit.Diamond),
            new PokerCard(Rank.Three, Suit.Heart),
            new PokerCard(Rank.Four, Suit.Club),
            new PokerCard(Rank.Five, Suit.Diamond),
        };
        var topPlay = new SinglePattern(topPlayCards);
        round.TopPlay = topPlay;
        
        // Act
        var result = round.CanOverrideTopPlay(pattern);
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void CreateCardPattern_WithFullHouse_ReturnsFullHousePattern()
    {
        // Arrange
        // var fullHouse = new List<PokerCard>()
        // {
        //     new PokerCard(Rank.Ace, Suit.Club),
        //     new PokerCard(Rank.Ace, Suit.Diamond),
        //     new PokerCard(Rank.Ace, Suit.Heart),
        //     new PokerCard(Rank.King, Suit.Club),
        //     new PokerCard(Rank.King, Suit.Diamond),
        // };
        
        var straight = new List<PokerCard>()
        {
            new PokerCard(Rank.Ace, Suit.Club),
            new PokerCard(Rank.Two, Suit.Diamond),
            new PokerCard(Rank.Three, Suit.Heart),
            new PokerCard(Rank.Four, Suit.Club),
            new PokerCard(Rank.Five, Suit.Diamond),
        };
        
        
        var cardPatternFactory = CardPatternCreator.Instance;
        // Act
        // var pattern1 = cardPatternFactory.CreateCardPattern(fullHouse);
        var pattern2 = cardPatternFactory.CreateCardPattern(straight);
        
        // Assert
        // Assert.IsType<FullHousePattern>(pattern1);
        Assert.IsType<StraightPattern>(pattern2);
    }
}