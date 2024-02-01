using CardGameFramework.Big2.CardPatterns;
using CardGameFramework.Big2.Models;
using ICardPattern = CardGameFramework.Big2.CardPatterns.ICardPattern;

namespace CardGameFramework.Big2.CardBuilders;

public class StraightConstructor: CardPatternConstructor
{
    public StraightConstructor(CardPatternConstructor? next) : base(next)
    {
    }

    protected override bool IsMatch(List<PokerCard> cards)
    {
        return StraightPattern.IsValid(cards);
    }

    protected override ICardPattern Create(List<PokerCard> cards)
    {
        return new StraightPattern(cards);
    }
}