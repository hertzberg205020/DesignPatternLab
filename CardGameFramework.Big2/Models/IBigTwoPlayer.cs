using CardGameFramework.Big2.CardBuilders;
using CardGameFramework.Big2.CardPatterns;
using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public interface IBigTwoPlayer
{
    HandCard HandCard { get; }

    BigTwoGame? Game { get; set; }

    string? Name { get; }
    
    TurnMove TakeTurn()
    {
        if (Game == null)
        {
            throw new InvalidOperationException("The game is not set.");
        }
        
        if (Game.CurRound == null)
        {
            throw new InvalidOperationException("The current round is not set.");
        }
        
        while (true)
        {
            HandCard.ShowAll();
            IList<int>? indices = null;
            try
            {
                indices = ParseInput();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                continue;
            }
            
            ArgumentNullException.ThrowIfNull(indices, nameof(indices));

            if (indices.Count == 1 && indices.First() == -1)
            {
                return TurnMove.Pass(this);
            }
            
            ICardPattern? cards;
            
            try
            {
                cards = CreateCardPattern(indices);
            }
            catch (Exception)
            {
                Console.WriteLine("此牌型不合法，請再嘗試一次。");
                continue;
            }
            
            ArgumentNullException.ThrowIfNull(cards, nameof(cards));
            
            if (!Game.CurRound.CanOverrideTopPlay(cards))
            {
                Console.WriteLine("此牌型不合法，請再嘗試一次。");
                continue;
            }
            
            return TurnMove.Play(this, cards);
        }
    }

    IList<int> ParseInput();
    

    /// <summary>
    /// 依所選擇要出的牌嘗試建立一個合法的牌型
    /// </summary>
    /// <param name="indices"></param>
    /// <returns></returns>
    ICardPattern? CreateCardPattern(IList<int> indices)
    {
        var cardPatternCreator = CardPatternCreator.Instance;
        var cards = indices.Select(index => HandCard.Cards[index]).ToList();
        return cardPatternCreator.CreateCardPattern(cards);
    }
}