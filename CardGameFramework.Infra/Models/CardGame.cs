namespace CardGameFramework.Infra.Models;

public abstract class CardGame<TCard>
    where TCard : ICard
{
    public readonly IDeck<TCard> Deck;
    public readonly List<CardPlayer<TCard>> Players;

    protected CardGame(Deck<TCard> deck, List<CardPlayer<TCard>> players)
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

    protected void OnPlayersNaming()
    {
        for (var i = 0; i < Players.Count; i++)
        {
            Players[i].NameSelf(i + 1);
        }
    }

    private void OnDeckShuffling()
    {
        Deck.Shuffle();
    }

    protected void OnCardsDealtToPlayers()
    {
        while (!Deck.IsEmpty())
        {
            foreach (var player in Players.Where(player => !IsDealComplete(player)))
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
    protected virtual bool IsDealComplete(CardPlayer<TCard> player)
    {
        return false;
    }

    protected virtual void OnInitiateGame()
    {
    }

    protected void OnGameRoundsStart()
    {
        while (!IsGameOver())
        {
            OnExecuteRound();
        }
    }

    protected abstract bool IsGameOver();
    protected abstract void OnExecuteRound();

    protected virtual void OnGameEnded()
    {
        var winners = IdentifyWinners();
        ShowWinners(winners);
    }
    
    protected abstract IReadOnlyCollection<CardPlayer<TCard>> IdentifyWinners();

    protected void ShowWinners(IReadOnlyCollection<CardPlayer<TCard>> winners)
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