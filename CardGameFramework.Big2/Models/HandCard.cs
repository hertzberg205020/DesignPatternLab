using System.Text;
using CardGameFramework.Big2.Enums;
using CardGameFramework.Big2.Helpers;
using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public class HandCard: HandOfCards<PokerCard>
{
    public HandCard() : base(13)
    {
    }
    
    public override void Add(PokerCard card)
    {
        // Find the index of the first card that is greater than the given card
        var index = Cards.FindIndex(c => c.Rank > card.Rank || (c.Rank == card.Rank && c.Suit > card.Suit));

        if (index == -1)
        {
            Cards.Add(card);
        }
        else
        {
            Cards.Insert(index, card);
        }
    }
    
    public override void AddRange(IEnumerable<PokerCard> cards)
    {
        foreach (var card in cards)
        {
            Add(card);
        }
    }
    
    public bool HasClub3()
    {
        return Cards.Any(card => card is {Rank: Rank.Three, Suit: Suit.Club});
    }
    
    public void ShowAll()
    {
        // 呈現上方 Index 需要的空白字元數量
        // 1 whitespace + (card length - index length)  
        // 0    1    2    3    4    5    6    7     8    9    10   11   12
        // C[3] C[4] C[5] C[6] C[7] C[8] C[9] C[10] C[J] C[Q] C[K] C[A] C[2]
        const int space = 1;
        var spaceLengthLookup = Cards
            .Select((card, index) => new {index, card})
            .ToDictionary(t => t.index, t =>
            {
                var cardLength = t.card.ToString().Length;
                var indexLength = t.index.ToString().Length;
                return (cardLength - indexLength) + space;
            });
        
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < Cards.Count; i++)
        {
            var whiteSpaceLength = spaceLengthLookup[i];
            // Console.Write(i.ToString() + Enumerable.Repeat(' ', whiteSpaceLength));
            // Console.Write(i.ToString() + new string(' ', whiteSpaceLength));
            
            stringBuilder.Append(i.ToString());
            stringBuilder.Append(' ', whiteSpaceLength);
        }
        
        Console.Write(stringBuilder.ToString().Trim());
        Console.WriteLine();
        
        Console.WriteLine(string.Join(' ', Cards));
    }

    public void RemoveRange(IEnumerable<PokerCard> cards)
    {
        // Cards.RemoveAll(card => cards.Contains(card));
        Cards.RemoveAll(cards.Contains);
    }
}