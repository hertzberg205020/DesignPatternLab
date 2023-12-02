using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public class Big2GameAiPlayer: AiCardPlayer<PokerCard>, IBig2GamePlayer
{
    public int Points { get; set; }
    
    public PokerCard ShowCard()
    {
        var random = Random.Shared;
        var index = random.Next(0, HandOfCards.Cards.Count);
        return PlayCard(index);
    }
}