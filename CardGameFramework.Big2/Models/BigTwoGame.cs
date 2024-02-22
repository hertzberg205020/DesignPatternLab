using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public class BigTwoGame: CardGame<PokerCard>
{
    public Round? CurRound { get; set; }
    
    public IBigTwoPlayer? TopPlayer { get; set; }

    public BigTwoGame(IDeck<PokerCard> deck, List<CardPlayer<PokerCard>> players) : base(deck, players)
    {
        foreach (var player in players)
        {
            player.CardGame = this;
            if (player is IBigTwoPlayer bigTwoPlayer)
            {
                bigTwoPlayer.Game = this;
            }
        }
    }

    public override bool IsDealComplete(CardPlayer<PokerCard> player)
    {
        return player.HandOfCards.Cards.Count >= 13;
    }

    /// <summary>
    /// 找出持有梅花3的玩家
    /// </summary>
    public override void OnInitiateGame()
    {
        foreach (var player in Players.Select(p => p as IBigTwoPlayer))
        {
            if (player.HandCard.HasClub3())
            {
                TopPlayer = player;
                break;
            }
        }
    }

    public override bool IsGameOver()
    {
        return Players.Any(p => p.HandOfCards.Cards.Count == 0);
    }

    public override void OnExecuteRound()
    {
        ArgumentNullException.ThrowIfNull(TopPlayer, nameof(TopPlayer));
        CurRound = new Round(this, TopPlayer);
        TopPlayer = CurRound.Start();
    }

    public override IReadOnlyCollection<CardPlayer<PokerCard>> IdentifyWinners()
    {
        return new List<CardPlayer<PokerCard>>() {TopPlayer as CardPlayer<PokerCard>};
    }

    public override void OnGameEnded()
    {
        // 遊戲結束，遊戲的勝利者為 火球
        Console.WriteLine($"遊戲結束，遊戲的勝利者為 {TopPlayer?.Name}");
    }
}