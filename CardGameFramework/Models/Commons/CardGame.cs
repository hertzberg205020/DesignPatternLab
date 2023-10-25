namespace CardGameFramework.Models.Commons;

public abstract class CardGame<TCard> 
    where TCard : ICard
{
    public readonly Deck<TCard> Deck;
    protected readonly List<CardPlayer<TCard>> Players;

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
        OnGameRoundsStart();
        OnGameEnded();
    }

    protected abstract void OnGameEnded();


    protected abstract void OnGameRoundsStart();


    protected virtual void OnCardsDealtToPlayers()
    {
        while (!Deck.IsEmpty())
        {
            Players.ForEach(player => player.AddCardToHand(Deck.Draw()));
        }
    }

    private void OnDeckShuffling()
    {
        Deck.Shuffle();
    }

    private void OnPlayersNaming()
    {
        for (var i = 0; i < Players.Count; i++)
        {
            Players[i].NameSelf(i + 1);
        }
    }
    
}