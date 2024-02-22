using CardGameFramework.Infra.Models;

namespace CardGameFramework.Uno.Models;

public class UnoCardGame: CardGame<UnoCard>
{
    public readonly CardTable CardTable;
    
    private IUnoCardGamePlayer? CurrentPlayer { get; set; }
    
    public const int InitCardCount = 5;
    
    private bool IsGameContinue { get; set; } = true;
    
    public UnoCardGame(Deck<UnoCard> deck, List<CardPlayer<UnoCard>> players, CardTable cardTable) : 
        base(deck, players)
    {
        CardTable = cardTable;
        CardTable.Game = this;
    }

    public override bool IsDealComplete(CardPlayer<UnoCard> player)
    {
        return player.HandOfCards.Cards.Count > InitCardCount;
    }

    public override void OnInitiateGame()
    {
        CardTable.ChangeTopCard(Deck.Draw());
    }

    public override void OnExecuteRound()
    {
        foreach (var player in Players.Select(p => p as IUnoCardGamePlayer))
        {
            TakeTurn(player!);
            if (IsGameOver())
            {
                return;
            }
        }
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
            IsGameContinue = turnMove.IsContinue;
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

    public override bool IsGameOver()
    {
        return !IsGameContinue;
    }


    public override IReadOnlyCollection<CardPlayer<UnoCard>> IdentifyWinners()
    {
        return new []{CurrentPlayer as CardPlayer<UnoCard>}!;
    }
}