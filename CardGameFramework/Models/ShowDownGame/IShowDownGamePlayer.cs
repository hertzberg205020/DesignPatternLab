namespace CardGameFramework.Models.ShowDownGame;

public interface IShowDownGamePlayer
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