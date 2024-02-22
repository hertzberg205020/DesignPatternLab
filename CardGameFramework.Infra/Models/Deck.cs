using CardGameFramework.Infra.Helpers;

namespace CardGameFramework.Infra.Models;

public class Deck<TCard>: IDeck<TCard>
    where TCard : ICard
{
    private List<TCard> Cards { get; set; }

    public Deck()
    {
        Cards = new List<TCard>();

        var (typeU, typeV) = ReflectionHelper.GetConstructorGenericEnumTypes<TCard>();
        
        foreach (var category in Enum.GetValues(typeU))
        {
            foreach (var value in Enum.GetValues(typeV))
            {
                Cards.Add((TCard) Activator.CreateInstance(typeof(TCard), category, value));
            }
        }
        
        // Cards = new List<TCard>();
        // foreach (U category in Enum.GetValues(typeof(U)))
        // {
        //     foreach (V value in Enum.GetValues(typeof(V)))
        //     {
        //         Cards.Add((TCard) Activator.CreateInstance(typeof(TCard), category, value));
        //     }
        // }
    }

    public void Shuffle()
    {
        Random rnd = new();
        
        var n = Cards.Count;

        while (n > 1)
        {
            n--;
            var k = rnd.Next(n + 1);
            (Cards[k], Cards[n]) = (Cards[n], Cards[k]);
        }
    }

    public TCard Draw()
    {
        var card = Cards[0];
        Cards.RemoveAt(0);
        return card;
    }
    
    public bool IsEmpty()
    {
        return Cards.Count == 0;
    }
    
    public void Refill(List<TCard> cards)
    {
        Cards.AddRange(cards);
    }
}