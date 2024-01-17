using CardGameFramework.Infra.Models;

namespace CardGameFramework.Uno.Models;

public class UnoCardGame: CardGame<UnoCard>
{
    public readonly CardTable CardTable;
    
    private IUnoCardGamePlayer? CurrentPlayer { get; set; }
    
    public const int InitCardCount = 5;
    
    private bool IsGameEnded { get; set; } = false;
    
    public UnoCardGame(Deck<UnoCard> deck, List<CardPlayer<UnoCard>> players, CardTable cardTable) : 
        base(deck, players)
    {
        CardTable = cardTable;
        CardTable.Game = this;
    }
    
    protected override bool IsDealComplete(CardPlayer<UnoCard> player)
    {
        return player.HandOfCards.Cards.Count < InitCardCount;
    }

    protected override void OnInitiateGame()
    {
        CardTable.ChangeTopCard(Deck.Draw());
    }
    
    protected override void OnExecuteRound()
    {
        Players.OfType<IUnoCardGamePlayer>().ToList().ForEach(TakeTurn);
    }
    
    private void TakeTurn(IUnoCardGamePlayer player)
    {
        CurrentPlayer = player;
        Console.WriteLine($"{CurrentPlayer.Name}'s turn!");
        var turnMove = player.TakeTurn();
        if (turnMove.Card == null)
        {
            PlayerPass();
        }
        else
        {
            CardTable.ChangeTopCard(turnMove.Card);
            Console.WriteLine($"{CurrentPlayer.Name} lays {turnMove.Card}");
            IsGameEnded = !turnMove.IsContinue;
        }
    }
    
    private void PlayerPass()
    {
        Console.WriteLine($"{CurrentPlayer?.Name} passes.");
        if (!Deck.IsEmpty())
        {
            CardTable.RefillDeck();
            Deck.Shuffle();
        }
        CurrentPlayer?.AddCardToHand(Deck.Draw());
    }
    
    protected override bool IsGameOver()
    {
        return !IsGameEnded;
    }
    
    
    
    protected override IReadOnlyCollection<CardPlayer<UnoCard>> IdentifyWinners()
    {
        return new []{CurrentPlayer as CardPlayer<UnoCard>}!;
    }
}