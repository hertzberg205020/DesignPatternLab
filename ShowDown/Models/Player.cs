using System.Text;

namespace ShowDown.Models;

public abstract class Player
{
    public string Name { get; private set; }
    
    public int Point { get; private set; }
    
    public IList<Card> HandCards = new List<Card>();
    
    public ExchangeHandsRelationship ExchangeHandsRelationship { get; private set; }

    public bool HasExchangeHands { get; set; } = false;
    
    
    public void NameSelf(string name)
    {
        Name = name;
    }
    
    public void GainOnePoint()
    {
        Point += 1;
    }
    
    /// <summary>
    /// 玩家抽牌
    /// </summary>
    /// <param name="card"></param>
    public void DrawCard(Card card)
    {
        HandCards.Add(card);
    }
    
    /// <summary>
    /// 玩家出牌
    /// </summary>
    /// <returns></returns>
    public abstract Card Show();
    
    public abstract void MakeExchangeDecision(IList<Player> players);

    public void ExchangeHandCards(Player player)
    {
        if (HasExchangeHands)
        {
            throw new InvalidOperationException("You have already exchanged hands.");
        }
              
        HasExchangeHands = true;
        ExchangeHandsRelationship = new ExchangeHandsRelationship(this, player);
        
        // 被交換的玩家的手牌是否要在自身屬性記錄交換關係？
        (HandCards, player.HandCards) = (player.HandCards, HandCards);
    }
    
    /// <summary>
    /// 假如玩家已經交換過手牌，則更新交換關係的狀態 
    /// </summary>
    public void UpdateExchangeStateIfHasExchanged()
    {
        if (HasExchangeHands)
        {
            ExchangeHandsRelationship.UpdateState();
        } 
    }
}