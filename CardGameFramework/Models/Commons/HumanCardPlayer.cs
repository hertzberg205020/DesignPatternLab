using System.Text;

namespace CardGameFramework.Models.Commons;

public class HumanCardPlayer<TCard>: CardPlayer<TCard>
    where TCard: ICard
{
    public override void NameSelf(int order)
    {
        Console.WriteLine($"Player{order} Please enter your name: ");
        Name = Console.ReadLine();
    }
    
}