using CardGameFramework.Infra.Helpers;

namespace CardGameFramework.Infra.Models;

public class Deck<TCard> 
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
        var random = new Random();
        for (var i = 0; i < Cards.Count; i++)
        {
            var j = random.Next(i, Cards.Count);
            (Cards[i], Cards[j]) = (Cards[j], Cards[i]);
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