using CardGameFramework.Models.Commons;

namespace CardGameFramework.Models.ShowDownGame;

public class ShowDownGame : CardGame<PokerCard>
{
    public const int NumOfRounds = 13;
    private readonly List<TurnMove> _turnMoves = new();

    public ShowDownGame(Deck<PokerCard> deck, List<CardPlayer<PokerCard>> players)
        : base(deck, players)
    {
    }

    protected override void OnGameRoundsStart()
    {
        for (int i = 0; i < NumOfRounds; i++)
        {
            // 使用 OfType<T>()，僅會返回可以被成功轉型的物件
            Players.OfType<IShowDownGamePlayer>().ToList().ForEach(TakeTurn);
            ShowDown();
            _turnMoves.Clear();
        }
    }

    private void TakeTurn(IShowDownGamePlayer player)
    {
        Console.WriteLine($"It is {player.Name}'s turn.");
        var turnMove = player.TakeTurn();
        _turnMoves.Add(turnMove);
    }

    private void ShowDown()
    {
        PrintShowCards();
        var maxCard = _turnMoves.Max(t => t.Card);
        var winnerTurnMove = _turnMoves.FirstOrDefault(turnMove => Equals(turnMove.Card, maxCard));
        var winner = winnerTurnMove!.Player;
        winner.Points++;
        Console.WriteLine($"{winner.Name} wins this round.");
    }

    private void PrintShowCards()
    {
        foreach (var turnMove in _turnMoves)
        {
            Console.WriteLine($"{turnMove.Player.Name} shows {turnMove.Card}");
        }
    }

    protected override void OnGameEnded()
    {
        
        var sortedPlayers = Players.OfType<IShowDownGamePlayer>()
            .OrderByDescending(player => player.Points)
            .ToList();

        foreach (var player in sortedPlayers)
        {
            Console.WriteLine($"{player.Name} has {player.Points} points.");
        }

        var topScore = sortedPlayers.First().Points;
        var winners = sortedPlayers.Where(player => player.Points == topScore).ToList();

        ShowWinners(winners);
    }

    private void ShowWinners(List<IShowDownGamePlayer> winners)
    {
        if (winners.Count == 1)
        {
            Console.WriteLine($"{winners.First().Name} wins the game.");
        }
        else
        {
            string winnerNames = string.Join(", ", winners.Select(w => w.Name));
            Console.WriteLine($"It's a tie! Winners are: {winnerNames}.");
        }
    }
}