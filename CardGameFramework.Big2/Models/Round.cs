using System.Reflection;
using CardGameFramework.Big2.CardPatterns;
using CardGameFramework.Big2.Enums;
using CardGameFramework.Big2.Exceptions;

namespace CardGameFramework.Big2.Models;

public class Round
{
    public BigTwoGame? Game { get; set; }

    public bool IsFirstRound { get; set; }

    public List<IBigTwoPlayer>? OrderOfPlayers { get; set; }

    public ICardPattern? TopPlay { get; set; }

    public IBigTwoPlayer TopPlayer { get; set; }

    private int CountOfContinuousPass { get; set; } = 0;

    public bool IsGameContinue { get; set; } = true;

    private bool IsRoundContinue() => IsGameContinue && CountOfContinuousPass < 3;

    private int CurrentPlayerIndex { get; set; } = -1;

    public Round(BigTwoGame? game, IBigTwoPlayer topPlayer, bool isFirstRound = false)
    {
        Game = game;
        TopPlayer = topPlayer;
        IsFirstRound = isFirstRound;
    }

    public IBigTwoPlayer Start()
    {
        Console.WriteLine("新的回合開始了。");
        DeterminePlayerOrder();
        ArgumentNullException.ThrowIfNull(OrderOfPlayers, nameof(OrderOfPlayers));

        while (IsRoundContinue())
        {
            var currentPlayer = GetNextPlayer().First();
            Console.WriteLine($"輪到{currentPlayer.Name}了");
            TakeTurn(currentPlayer);
        }

        return TopPlayer;
    }

    private void DeterminePlayerOrder()
    {
        var players = Game?.Players.Select(x => (IBigTwoPlayer)x).ToList();
        ArgumentNullException.ThrowIfNull(players, nameof(players));
        var topPlayerIndex = players.IndexOf(TopPlayer);

        OrderOfPlayers = players
            .Skip(topPlayerIndex)
            .Concat(players.Take(topPlayerIndex))
            .ToList();
    }

    private IEnumerable<IBigTwoPlayer> GetNextPlayer()
    {
        ArgumentNullException.ThrowIfNull(OrderOfPlayers, nameof(OrderOfPlayers));

        var nextPlayerIndex = (CurrentPlayerIndex + 1) % OrderOfPlayers.Count;
        CurrentPlayerIndex = nextPlayerIndex;
        yield return OrderOfPlayers[nextPlayerIndex];
    }

    private void TakeTurn(IBigTwoPlayer player)
    {
        try
        {
            var turnMove = player.TakeTurn();

            // 首輪玩家不能 PASS
            if (TopPlayer == player && TopPlay == null && turnMove.IsPass)
            {
                throw new TopPlayerPassOnFirstTurnException();
            }

            if (turnMove.IsPass)
            {
                HandlePass(player);
            }

            if (turnMove is { IsPass: false, Cards: not null })
            {
                // 第一局玩家要出含有梅花3的牌型
                if (IsFirstRound)
                {
                    if (!turnMove.Cards.Contents.Any(p => p.Suit == Suit.Club && p.Rank == Rank.Three))
                    {
                        throw new FirstRoundMustPlayClub3Exception();
                    }
                }

                HandleValidPlay(turnMove);
            }
        }
        catch (TopPlayerPassOnFirstTurnException e)
        {
            Console.WriteLine("你不能在新的回合中喊 PASS");
            TakeTurn(player);
        }
        catch (FirstRoundMustPlayClub3Exception e)
        {
            Console.WriteLine("此牌型不合法，請再嘗試一次。");
            TakeTurn(player);
        }
        catch (NoSuchPatternException e)
        {
            Console.WriteLine("此牌型不合法，請再嘗試一次。");
            TakeTurn(player);
        }
    }

    public bool CanOverrideTopPlay(CardPatterns.ICardPattern play)
    {
        // 若牌桌上沒有頂牌，則可以出任何牌
        if (TopPlay == null)
        {
            return true;
        }

        Type topType = TopPlay.GetType();
        Type playType = play.GetType();

        // 型別一致 且 play 牌型 比 top 大 才能出牌
        // 使用反射設使用 CompareTo 方法
        if (topType == playType)
        {
            var method = topType.GetMethod("CompareTo");
            if (method != null)
            {
                var result = method.Invoke(play, new object[] { TopPlay });
                if (result != null)
                {
                    return (int)result > 0;
                }
            }
        }

        return false;
    }

    private void HandlePass(IBigTwoPlayer player)
    {
        // 玩家 地瓜球 PASS.
        Console.WriteLine($"玩家 {player.Name} PASS.");
        CountOfContinuousPass++;
    }

    private void HandleValidPlay(TurnMove turnMove)
    {
        ArgumentNullException.ThrowIfNull(turnMove.Cards, nameof(turnMove.Cards));
        var player = turnMove.Player;
        var play = turnMove.Cards;
        // 玩家 水球 打出了 單張 C[5]
        // 玩家 保齡球 打出了 順子 H[J] S[Q] C[K] C[A] C[2]
        var patternName = play.GetType()
            .GetProperty("PatternName", BindingFlags.Public | BindingFlags.Static)
            ?.GetValue(play);
        Console.WriteLine($"玩家 {player.Name} 打出了 {patternName} {play}");
        TopPlay = play;
        TopPlayer = player;
        CountOfContinuousPass = 0;
        IsGameContinue = turnMove.IsGameContinue;
    }
}