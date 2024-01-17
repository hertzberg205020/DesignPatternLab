using CardGameFramework.Infra.Models;
using CardGameFramework.Models.ShowDownGame;

namespace CardGameFramework.Showdown.Models;

public class ShowDownGame : CardGame<PokerCard>
{
    private const int NumOfRounds = 13;
    private int _curRound = 0;
    private readonly List<TurnMove> _turnMoves = new();

    public ShowDownGame(Deck<PokerCard> deck, List<CardPlayer<PokerCard>> players)
        : base(deck, players)
    {
        if (players.Any(player => player is not IShowDownGamePlayer))
        {
            throw new ArgumentException("Player must be IShowDownGamePlayer");
        }
    }
    
    protected override bool IsGameOver()
    {
        return _curRound < NumOfRounds;
    }

    protected override void OnExecuteRound()
    {
        Players.OfType<IShowDownGamePlayer>().ToList().ForEach(TakeTurn);
        ShowDown();
        _turnMoves.Clear();
        _curRound++;
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
    
    protected override IReadOnlyCollection<CardPlayer<PokerCard>> IdentifyWinners()
    {
        var sortedPlayers = Players.OfType<IShowDownGamePlayer>()
            .OrderByDescending(player => player.Points)
            .ToList();

        var topScore = sortedPlayers.First().Points;
        var winners = sortedPlayers.Where(player => player.Points == topScore).Cast<CardPlayer<PokerCard>>().ToList();;

        return winners;
    }
}