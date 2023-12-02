using CardGameFramework.Infra.Models;

namespace CardGameFramework.Uno.Models;

public class UnoGameHumanPlayer : HumanCardPlayer<UnoCard>, IUnoCardGamePlayer
{
    public CardTable CardTable => ((UnoCardGame)CardGame).CardTable;
    
    public UnoCard? LayCard()
    {
        while (true)
        {
            Console.WriteLine($"the top card is {CardTable.TopUnoCard}");
            
            DisplayCardsSelections();
            Console.WriteLine("Please enter the index of card you want to lay or -1 (pass): ");
            
            var cardIndex = GetCardIndexFromUser();
            
            if(cardIndex == -1) return null;

            var selectedCard = HandOfCards.Cards[cardIndex] as UnoCard;

            if (((IUnoCardGamePlayer)this).IsValidCardToLay(selectedCard))
            {
                return PlayCard(cardIndex);
            }
            
        }
    }


    private int GetCardIndexFromUser()
    {
        while (true)
        {
            Console.WriteLine("Please enter the card you want to lay: ");
            var userInput = Console.ReadLine();

            if (int.TryParse(userInput, out var index) && index >= -1 && index < HandOfCards.Cards.Count)
            {
                return index;
            }

            Console.WriteLine("Invalid index for input!");
        }
    }
}