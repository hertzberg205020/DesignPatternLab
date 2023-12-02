namespace CardGameFramework.Infra.Models;

public class HandOfCards<TCard> 
    where TCard: ICard
{
    public List<TCard> Cards { get; } = new List<TCard>();
    
    public void Add(TCard card)
    {
        Cards.Add(card);
    }
    
    
    public TCard PlaceCard(int index)
    {
        if (index < 0 || index >= Cards.Count)
        {
            throw new IndexOutOfRangeException("The index is out of range.");
        }
        
        var target = Cards[index];
        Cards.RemoveAt(index);
        return target;
    }
    
}