namespace CardGameFramework.Infra.Models;

public class HumanCardPlayer<TCard>: CardPlayer<TCard>
    where TCard: ICard
{
    protected HumanCardPlayer() { }
    protected HumanCardPlayer(HandOfCards<TCard> handOfCards) : base(handOfCards)
    {
    }
    
    public override void NameSelf(int order)
    {
        var name = Console.ReadLine();
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
    }
    
}