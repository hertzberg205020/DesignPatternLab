using System.Collections;

namespace CardGameFramework.Infra.Models;

public class HandOfCards<TCard> : IEnumerable<TCard>
    where TCard: ICard
{
    public List<TCard> Cards { get; }
    
    public HandOfCards(int size = 5)
    {
        Cards = new List<TCard>(size);
    }
    
    public TCard this[int index] => Cards[index];
        
    public IEnumerator<TCard> GetEnumerator()
    {
        return Cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public virtual void Add(TCard card)
    {
        Cards.Add(card);
    }
    
    public virtual TCard PlaceCard(int index)
    {
        if (index < 0 || index >= Cards.Count)
        {
            throw new IndexOutOfRangeException("The index is out of range.");
        }
        
        var target = Cards[index];
        Cards.RemoveAt(index);
        return target;
    }
    
    public virtual void AddRange(IEnumerable<TCard> cards)
    {
        Cards.AddRange(cards);
    }
    
    public virtual void Clear()
    {
        Cards.Clear();
    }
    
}