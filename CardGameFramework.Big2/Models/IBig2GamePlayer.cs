namespace CardGameFramework.Big2.Models;

public interface IBig2GamePlayer
{
    public int Points { get; set; }
    
    public string? Name { get; }
    
    void NameSelf(int order);

    TurnMove TakeTurn()
    {
        var card = ShowCard();
        return new TurnMove(this, card);        
    }
    
    PokerCard ShowCard();
    
    void AddCardToHand(PokerCard card);

    void GainOnePoint()
    {
        Points++;
    }
}