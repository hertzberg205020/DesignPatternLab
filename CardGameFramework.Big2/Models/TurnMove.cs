
using CardGameFramework.Big2.CardPatterns;

namespace CardGameFramework.Big2.Models;

public class TurnMove
{
    public readonly IBigTwoPlayer Player;
    
    public readonly ICardPattern? Cards;
    
    public readonly bool IsPass;
    
    public bool IsGameContinue => Player.HandCard.Cards.Count > 0;
    
    private TurnMove(IBigTwoPlayer player, ICardPattern? cards, bool isPass)
    {
        Player = player;
        Cards = cards;
        IsPass = isPass;
    }
    
    public static TurnMove Pass(IBigTwoPlayer player)
    {
        return new TurnMove(player, null, true);
    }
    
    public static TurnMove Play(IBigTwoPlayer player, ICardPattern cards)
    {
        player.HandCard.RemoveRange(cards.Contents);
        return new TurnMove(player, cards, false);
    }
    
    public void Revert()
    {
        if (IsPass || Cards == null)
        {
            return;
        }
        
        Player.HandCard.AddRange(Cards.Contents);
    }
}