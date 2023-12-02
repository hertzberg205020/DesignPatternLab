namespace CardGameFramework.Infra.Models;

public class AiCardPlayer<TCard>: CardPlayer<TCard>
    where TCard: ICard
{
    public override void NameSelf(int order)
    {
        Name = $"Player{order}";
    }
}