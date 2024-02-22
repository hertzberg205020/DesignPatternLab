using CardGameFramework.Big2.Enums;
using CardGameFramework.Big2.Models;
using CardGameFramework.Infra.Extensions;

namespace CardGameFramework.Big2.Helpers;

public static class CardHelper
{
    public static readonly Dictionary<string, Rank> RANK_CACHE = new ()
    {
        {"2", Rank.Two},
        {"3", Rank.Three},
        {"4", Rank.Four},
        {"5", Rank.Five},
        {"6", Rank.Six},
        {"7", Rank.Seven},
        {"8", Rank.Eight},
        {"9", Rank.Nine},
        {"10", Rank.Ten},
        {"J", Rank.Jack},
        {"Q", Rank.Queen},
        {"K", Rank.King},
        {"A", Rank.Ace},
    };
    
    public static readonly Dictionary<string, Suit> SUIT_CACHE = new ()
    {
        {"C", Suit.Club},
        {"D", Suit.Diamond},
        {"H", Suit.Heart},
        {"S", Suit.Spade},
    };
    
    private static string FormatCard(PokerCard card)
    {
        // 假設最長的牌表示形式為 "D[10]"，長度為 6，包括括號和方括號
        return $"{card.Suit.GetDisplayName()}[{card.Rank.GetDisplayName()}]".PadRight(6);
    }

    private static string FormatIndex(int index)
    {
        // 確保索引和牌的長度一致，這裡使用 6 作為寬度，因為牌的格式化長度為 6
        return $"{index}".PadRight(6);
    }
    
    public static void PrintCards(List<PokerCard> cards)
    {
        // 輸出索引
        for (int i = 0; i < cards.Count; i++)
        {
            Console.Write(FormatIndex(i));
        }
        Console.WriteLine();

        // 輸出牌
        foreach (var card in cards)
        {
            Console.Write(FormatCard(card));
        }
    }
}