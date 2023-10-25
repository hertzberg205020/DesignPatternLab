using CardGameFramework.Enums;
using CardGameFramework.Models.UnoGame;

namespace CardGameFramework.Tests.Unit;

public class UnoCardTests
{
    [Fact]
    public void IsMatchCard_TheSameColor_True()
    {
        var card1 = new UnoCard(Color.Red, Number.One);
        var card2 = new UnoCard(Color.Red, Number.One);
        
        var res = card1.MatchCard(card2);
        
        Assert.True(res);
    }
    
    [Fact]
    public void IsMatchCard_TheSameNumber_True()
    {
        var card1 = new UnoCard(Color.Red, Number.One);
        var card2 = new UnoCard(Color.Blue, Number.One);
        
        var res = card1.MatchCard(card2);
        
        Assert.True(res);
    }
}