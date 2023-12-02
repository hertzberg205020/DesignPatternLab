using CardGameFramework.Infra.Models;

namespace CardGameFramework.Showdown.Models;

public class ShowDownGameHumanPlayer: HumanCardPlayer<PokerCard>, IShowDownGamePlayer 
{
    public int Points { get; set; }
    
    public PokerCard ShowCard()
    {
        while (true)
        {
            DisplayCardsSelections();
            Console.WriteLine("Please enter the card you want to show: ");
            var indexOfCard = Console.ReadLine();
            if (int.TryParse(indexOfCard, out var index))
            {
                if (index >= 0 && index < HandOfCards.Cards.Count)
                {
                    return PlayCard(index);
                }
            }
            Console.WriteLine("Invalid input, please try again.");
        }
    }
    
}