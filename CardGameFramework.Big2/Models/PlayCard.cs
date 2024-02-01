using System.Collections;
using CardGameFramework.Big2.Enums;

namespace CardGameFramework.Big2.Models;

public sealed class PlayCards:  IEnumerable<PokerCard>
{
    public readonly IReadOnlyList<PokerCard> Contents;
    public CardPatternType? PatternType { get; set; }

    public PlayCards(params PokerCard[] cards)
    {
        Contents = cards.ToList();
    }

    // 索引器
    public PokerCard this[int index] => Contents[index];

    // 實現 IEnumerable<PokerCard>
    public IEnumerator<PokerCard> GetEnumerator()
    {
        return Contents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}