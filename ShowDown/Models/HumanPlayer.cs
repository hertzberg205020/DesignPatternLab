using System.Text;

namespace ShowDown.Models;

public class HumanPlayer: Player
{
    
    /// <summary>
    /// 人類玩家出牌
    /// </summary>
    /// <returns></returns>
    public override Card Show()
    {
        int maxIndex = HandCards.Count;
        DisplayHandCards();
        Console.WriteLine($"請輸入要出的牌的編號，範圍為 0 到 {maxIndex}");
        var input = GetValidIndex(0, HandCards.Count, "請輸入要出的牌的編號");
        var card = HandCards[input];
        HandCards.RemoveAt(input);
        return card;
    }


    private int GetValidIndex(int lowerBound, int higherBound, string prompt)
    {
        if (lowerBound >= higherBound)
        {
            throw new ArgumentException("lowerBound 必須小於 higherBound");
        }
        
        while (true)
        {
            Console.Write(prompt + " ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out var index) && 
                index >= lowerBound &&
                index < higherBound)
            {
                return index;
            }

            Console.WriteLine($"輸入不正確，請再次輸入一個在 {lowerBound} 到 {higherBound} 之間的數字!");
        }
    }

    /// <summary>
    /// 顯示手牌
    /// </summary>
    /// <returns></returns>
    public string DisplayHandCards()
    {
        StringBuilder result = new StringBuilder();
        int cardCount = HandCards.Count;
        
        for (var i = 0; i < cardCount; i++)
        {
            var formattedIndex = i.ToString("00"); 
            result.Append(formattedIndex).Append('\t');
        }
        
        result.AppendLine();
        
        foreach (var card in HandCards)
        {
            result.Append(card.ToString()).Append('\t');
        }

        return result.ToString();
    }
    

    public override void MakeExchangeDecision(IList<Player> players)
    {
        var otherPlayers = players.Where(p => p != this).ToList();
    
        if (!HasExchangeHands)
        {
            if (InquireWantToExchangeHandCards())
            {
                var player = InquireWhichPlayerToExchangeHandCards(otherPlayers);
                ExchangeHandCards(player);
            }
        }
        else
        {
            if (ExchangeHandsRelationship.IsTimeToRevert())
            {
                RevertHandSwap();
            }
        }
    }
    
    
    private bool InquireWantToExchangeHandCards()
    {
        Console.WriteLine("請問要交換手牌嗎？(Y/N)");

        while (true)
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "Y" or "y":
                    return true;
                case "N" or "n":
                    return false;
                default:
                    Console.WriteLine("輸入不正確，請再次輸入 Y 或 N!");
                    break;
            }
        }
    }
    
    private Player InquireWhichPlayerToExchangeHandCards(List<Player> otherPlayers)
    {
        Console.WriteLine("請問要和哪位玩家交換手牌？(請輸入玩家名稱)");
        ShowAllPlayers(otherPlayers);
        return GetPlayerByName(otherPlayers);
    }

    private void ShowAllPlayers(List<Player> otherPlayers)
    {
        foreach (var player in otherPlayers)
        {
            Console.WriteLine(player.Name);
        }
    }

    private Player GetPlayerByName(List<Player> otherPlayers)
    {
        Player? player;

        while (true)
        {
            var input = Console.ReadLine();
        
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("輸入不正確，請再次輸入玩家名稱！");
                continue;
            }
        
            player = otherPlayers.SingleOrDefault(p => p.Name == input);
        
            if (player != null)
            {
                break;
            }

            Console.WriteLine($"找不到名稱為 {input} 的玩家，請再次輸入玩家名稱！");
        }
    
        return player;
    }
    
}