using CardGameFramework.Big2.CardPatterns;
using CardGameFramework.Big2.Exceptions;
using CardGameFramework.Big2.Models;
using ICardPattern = CardGameFramework.Big2.CardPatterns.ICardPattern;

namespace CardGameFramework.Big2.CardBuilders;

public abstract class CardPatternConstructor
{
    public CardPatternConstructor? Next { get; set; }

    public CardPatternConstructor(CardPatternConstructor? next)
    {
        Next = next;
    }
    
    public ICardPattern Construct(List<PokerCard> cards)
    {
        if (IsMatch(cards))
        {
            return Create(cards);
        }

        if (Next != null)
        {
            return Next.Construct(cards);
        }

        throw new NoSuchPatternException();
    }

    protected abstract bool IsMatch(List<PokerCard> cards);
    
    protected abstract ICardPattern Create(List<PokerCard> cards);
}