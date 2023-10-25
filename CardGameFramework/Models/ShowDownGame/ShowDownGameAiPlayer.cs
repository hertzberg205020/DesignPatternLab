using CardGameFramework.Models.Commons;

namespace CardGameFramework.Models.ShowDownGame;

public class ShowDownGameAiPlayer: AiCardPlayer<PokerCard>, IShowDownGamePlayer
{
    public int Points { get; set; }
    
    public PokerCard ShowCard()
    {
        var random = Random.Shared;
        var index = random.Next(0, HandOfCards.Cards.Count);
        return PlayCard(index);
    }
}