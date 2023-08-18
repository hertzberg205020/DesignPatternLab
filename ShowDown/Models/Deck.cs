using ShowDown.Enums;

namespace ShowDown.Models;

public class Deck
{
    public IList<Card> Cards { get; private set; } = new List<Card>();
    
    public Deck()
    {
        foreach (var suit in Enum.GetValues<Suit>())
        {
            foreach (var rank in Enum.GetValues<Rank>())
            {
                Cards.Add(new Card(suit, rank));
            }
        }
    }
    
    /// <summary>
    /// 洗牌
    /// </summary>
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
    
    /// <summary>
    /// 牌堆份配撲克牌給玩家
    /// </summary>
    /// <param name="player"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void DealCardToPlayer(Player player)
    {
        if (!Cards.Any())
        {
            throw new InvalidOperationException("No more cards to deal.");
        }
        
        var card = Cards[0];
        Cards.RemoveAt(0);
        player.HandCards.Add(card);
    }
}