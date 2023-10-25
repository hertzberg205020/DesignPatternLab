using CardGameFramework.Enums;
using CardGameFramework.Models.ShowDownGame;

namespace CardGameFramework.Tests.Unit;

public class CardTests
{
    [Fact]
    public void Equals_TheSameCard_True()
    {
        var card1 = new PokerCard(Suit.Clubs, Rank.Ace);
        var card2 = new PokerCard(Suit.Clubs, Rank.Ace);

        var res = card1.Equals(card2);
        
        Assert.True(res);
    }
    
    [Fact]
    public void Equals_DifferentCard_False()
    {
        var card1 = new PokerCard(Suit.Clubs, Rank.Ace);
        var card2 = new PokerCard(Suit.Clubs, Rank.Two);

        var res = card1.Equals(card2);
        
        Assert.False(res);
    }
}