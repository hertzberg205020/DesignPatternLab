using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.CardPatterns;

public interface ICardPattern
{
    IList<PokerCard> Contents { get; set; }
    static abstract bool IsValid(IList<PokerCard> cards);
}