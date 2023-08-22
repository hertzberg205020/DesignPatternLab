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
        var handCards = GetHandCards();
        Console.WriteLine(handCards);
        Console.WriteLine($"請輸入要出的牌的編號，範圍為 0 到 {maxIndex-1}");
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

            Console.WriteLine($"輸入不正確，請再次輸入一個在 {lowerBound} 到 {higherBound-1} 之間的數字!");
        }
    }

    /// <summary>
    /// 顯示手牌
    /// </summary>
    /// <returns></returns>
    public string GetHandCards()
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
        Console.WriteLine("請問要和哪位玩家交換手牌？(請輸入編號 數字)");
        ShowAllPlayers(otherPlayers);
        return GetPlayerByIndex(otherPlayers);
    }

    private void ShowAllPlayers(List<Player> otherPlayers)
    {
        var index = 0;
        foreach (var player in otherPlayers)
        {
            Console.WriteLine($"{player.Name}--{index}");
            index++;
        }
    }

    private Player GetPlayerByIndex(List<Player> otherPlayers)
    {
        while (true)
        {
            var input = Console.ReadLine();

            if (IsValidInput(input, otherPlayers.Count, out var index))
            {
                return otherPlayers[index];
            }

            Console.WriteLine("輸入不正確，請再次輸入玩家編號！");
        }
    }
    
    private bool IsValidInput(string? input, int maxIndex, out int parsedIndex)
    {
        if (!string.IsNullOrWhiteSpace(input) && 
            int.TryParse(input, out parsedIndex) && 
            parsedIndex < maxIndex)
        {
            return true;
        }

        parsedIndex = -1; 
        return false;
    }
    
}