using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public class Big2GameHumanPlayer: HumanCardPlayer<PokerCard>, IBig2GamePlayer 
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