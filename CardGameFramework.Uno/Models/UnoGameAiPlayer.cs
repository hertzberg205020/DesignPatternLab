using CardGameFramework.Infra.Models;

namespace CardGameFramework.Uno.Models;

public class UnoGameAiPlayer: AiCardPlayer<UnoCard>, IUnoCardGamePlayer
{
    public CardTable CardTable => ((UnoCardGame) CardGame).CardTable;
    
    public UnoCard? LayCard()
    {
        Console.WriteLine($"the top card is {CardTable.TopUnoCard}");
        DisplayCardsSelections();
        foreach (UnoCard card in HandOfCards.Cards)
        {
            if (((IUnoCardGamePlayer) this).IsValidCardToLay(card))
            {
                return PlayCard(HandOfCards.Cards.IndexOf(card));
            }
        }

        return null;
    }
    
}