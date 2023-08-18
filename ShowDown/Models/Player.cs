using System.Text;

namespace ShowDown.Models;

public abstract class Player
{
    public string Name { get; private set; }
    public int Point { get; private set; }
    
    public readonly IList<Card> HandCards = new List<Card>();
    
    public void NameSelf(string name)
    {
        Name = name;
    }
    
    public void GainOnePoint()
    {
        Point += 1;
    }
    
    public void DrawCard(Card card)
    {
        HandCards.Add(card);
    }
    
    /// <summary>
    /// 顯示
    /// </summary>
    /// <returns></returns>
    public string DisplayHandCards()
    {
        StringBuilder result = new StringBuilder();
        int cardCount = HandCards.Count;

        // 上半部: 索引
        for (int i = 0; i < cardCount; i++)
        {
            string formattedIndex = i.ToString("00"); 
            result.Append(formattedIndex).Append('\t');
        }
        
        result.AppendLine();
        
        foreach (var card in HandCards)
        {
            result.Append(card.ToString()).Append('\t');
        }

        return result.ToString();
    }
}