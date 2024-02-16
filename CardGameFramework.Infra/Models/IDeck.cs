namespace CardGameFramework.Infra.Models;

public interface IDeck<TCard> 
    where TCard : ICard
{
    void Shuffle();
    TCard Draw();
    bool IsEmpty();
    void Refill(List<TCard> cards);
}