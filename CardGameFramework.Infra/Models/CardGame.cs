namespace CardGameFramework.Infra.Models;

public abstract class CardGame<TCard>
    where TCard : ICard
{
    public readonly IDeck<TCard> Deck;
    public readonly List<CardPlayer<TCard>> Players;
    private int _playerIndex = 0;

    protected CardGame(IDeck<TCard> deck, List<CardPlayer<TCard>> players)
    {
        Deck = deck;
        Players = players;
        Players.ForEach(player => player.CardGame = this);
    }


    public void Run()
    {
        OnPlayersNaming();
        OnDeckShuffling();
        OnCardsDealtToPlayers();
        OnInitiateGame();
        OnGameRoundsStart();
        OnGameEnded();
    }

    public void OnPlayersNaming()
    {
        for (var i = 0; i < Players.Count; i++)
        {
            Players[i].NameSelf(i + 1);
        }
    }

    public void OnDeckShuffling()
    {
        Deck.Shuffle();
    }

    public void OnCardsDealtToPlayers()
    {
        
        while (!Deck.IsEmpty())
        {
            foreach (var player in Players)
            {
                player.AddCardToHand(Deck.Draw());
            }
            if (Players.All(IsDealComplete))
            {
                break;
            }
        }
    }
    

    /// <summary>
    /// 抽牌終止條件
    /// </summary>
    /// <returns></returns>
    public virtual bool IsDealComplete(CardPlayer<TCard> player)
    {
        return false;
    }

    public virtual void OnInitiateGame()
    {
    }

    public void OnGameRoundsStart()
    {
        while (!IsGameOver())
        {
            OnExecuteRound();
        }
    }

    public abstract bool IsGameOver();
    public abstract void OnExecuteRound();

    public virtual void OnGameEnded()
    {
        var winners = IdentifyWinners();
        ShowWinners(winners);
    }

    public abstract IReadOnlyCollection<CardPlayer<TCard>> IdentifyWinners();

    public void ShowWinners(IReadOnlyCollection<CardPlayer<TCard>> winners)
    {
        if (winners.Count == 1)
        {
            Console.WriteLine($"{winners.First().Name} wins the game.");
        }
        else
        {
            var winnerNames = string.Join(", ", winners.Select(w => w.Name));
            Console.WriteLine($"It's a tie! Winners are: {winnerNames}.");
        }
    }
}