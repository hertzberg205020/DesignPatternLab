using CardGameFramework.Big2.CardPatterns;
using CardGameFramework.Big2.Models;

namespace CardGameFramework.Big2.CardBuilders;

public class PairConstructor: CardPatternConstructor
{
    public PairConstructor(CardPatternConstructor? next) : base(next)
    {
    }

    protected override bool IsMatch(List<PokerCard> cards)
    {
        return PairPattern.IsValid(cards);
    }

    protected override ICardPattern Create(List<PokerCard> cards)
    {
        return new PairPattern(cards);
    }
}