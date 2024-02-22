using CardGameFramework.Big2.Models;
using ICardPattern = CardGameFramework.Big2.CardPatterns.ICardPattern;

namespace CardGameFramework.Big2.CardBuilders;

public class CardPatternCreator
{
    private static CardPatternCreator? _instance;
    private static readonly object Lock = new object();
    private readonly CardPatternConstructor _patternConstructor;

    private CardPatternCreator()
    {
        _patternConstructor = new FullHouseConstructor(
            new StraightConstructor(
                new PairConstructor(
                    new SingleConstructor(null))));
    }

    public static CardPatternCreator Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CardPatternCreator();
                    }
                }
            }
            return _instance;
        }
    }

    public ICardPattern? CreateCardPattern(List<PokerCard> cards)
    {
        return _patternConstructor.Construct(cards);
    }
}

