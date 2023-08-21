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

    protected void ExchangeHandCards(Player player)
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
    /// 將手牌交換回來
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void RevertHandExchange()
    {
        if (!HasExchangeHands)
        {
            throw new InvalidOperationException("You have not exchanged hands.");
        }
        
        if(ExchangeHandsRelationship.Round < 3)
        {
            throw new InvalidOperationException("交換手牌的關係會持續三回合後解除");
        }
        
        (HandCards, ExchangeHandsRelationship.OtherPlayer.HandCards) = (ExchangeHandsRelationship.OtherPlayer.HandCards, HandCards);
    }
    
    public bool IsTimeToRevertHandExchange()
    {
        return HasExchangeHands && ExchangeHandsRelationship.Round >= 3;
    }
    
    public void RevertHandExchangeIfTime()
    {
        if (IsTimeToRevertHandExchange())
        {
            RevertHandExchange();
        }
    }
    
    
    public void UpdateExchangeState()
    {
        if (HasExchangeHands)
        {
            ExchangeHandsRelationship.AddOneRound();
        } 
    }
    
    public Card ExecuteTurnActions(IList<Player> players)
    { 
        RevertHandExchangeIfTime();
        MakeExchangeDecision(players);
        var card = Show();
        return card;
    }
    
}