namespace ShowDown.Models;

public class ShowDownGame
{
    private List<Player> Players { get; init; }

    private Deck Deck { get; init; }
    
    private int _currentRound = 0;

    private const int MaxNumberOfCardsOnHand = 13;
    
    private const int TotalRound = 13;

    public ShowDownGame()
    {
        Players = new List<Player>();
        Deck = new Deck();
    }
    
    public ShowDownGame(IList<Player> players, Deck deck)
    {
        Players = players.ToList();
        Deck = deck;
    }
    
    public void Start()
    {
        OnInit();
        OnCardDraw();
        OnPlayTurns();
        OnGameEnd();
    }

    /// <summary>
    /// 請 P1~P4 為自己取名(Name himself)
    /// 牌堆會進行洗牌 (Shuffle)
    /// </summary>
    public void OnInit()
    {
        var amountOfHumanPlayerCount =InvestigateHumanPlayerCount();
        CreatePlayers(amountOfHumanPlayerCount);
        NamingPlayers();
        Deck.Shuffle();
    }
    
    /// <summary>
    /// 抽牌階段
    /// </summary>
    public void OnCardDraw()
    {
        for (var i = 0; i < MaxNumberOfCardsOnHand; i++)
        {
            foreach (var player in Players)
            {
                Deck.DealCardToPlayer(player);
            }
        }
    }

    /// <summary>
    /// 每回合的遊戲輪流讓玩家打牌
    /// </summary>
    public void OnPlayTurns()
    {
        while (HasNextRound())
        {
            var round = InitializeRoundCardPlay();
            OnPlayerExchangeHandsStage();
            TakeATurn(round);
            ShowCards(round);
            OnRoundEnd(round);
        }
    }
    
    /// <summary>
    /// 遊戲結束，顯示贏家
    /// </summary>
    public void OnGameEnd()
    {
        var gameWinner = GetGameWinner();
        foreach (var player in gameWinner)
        {
            Console.WriteLine($"{player.Name} 贏得此次遊戲的勝利！");
        }
    }

    private int InvestigateHumanPlayerCount()
    {
        while (true)
        {
            Console.WriteLine("請輸入真實玩家人數：");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var count) && count >= 0)
            {
                return count;
            }
            Console.WriteLine("輸入不正確，請再次輸入一個大於 0 的數字!");
        }
    }

    private void NamingPlayers()
    {
        var index = 0;
        foreach (var player in Players)
        {
            index++;
            Console.WriteLine($"請輸入{index}號玩家名稱：");
            var name = Console.ReadLine();
            player.NameSelf(string.IsNullOrWhiteSpace(name) ? $"玩家{index}號" : name);
        }
    }
    
    private void CreatePlayers(int amountOfHumanPlayerCount)
    {
        for (var i = 0; i < amountOfHumanPlayerCount; i++)
        {
            Players.Add(new HumanPlayer());
        }
        
        for (var i = 0; i < 4 - amountOfHumanPlayerCount; i++)
        {
            Players.Add(new AiPlayer());
        }
    }

    private IDictionary<Player, Card?> InitializeRoundCardPlay()
    {
        return Players.ToDictionary(p => p, p => (Card?)null);
    }

    private void OnPlayerExchangeHandsStage()
    {
        PlayersRevertExchangeHandsIfTime();
        PlayersMakeExchangeDecision();
    }

    private void PlayersRevertExchangeHandsIfTime()
    {
        foreach (var player in Players)
        {
            player.UpdateExchangeStateIfHasExchanged();
        }
    }
    
    private void PlayersMakeExchangeDecision()
    {
        foreach (var player in Players)
        {
            player.MakeExchangeDecision(Players);
        }
    }

    private void TakeATurn(IDictionary<Player, Card?> round)
    {
        foreach (var player in Players)
        {
            var card = player.Show();
            round[player] = card;
            player.UpdateExchangeStateIfHasExchanged();
        }
    }

    private void ShowCards(IDictionary<Player, Card?> round)
    {
        foreach (var player in round.Keys)
        {
            Console.WriteLine($"{player.Name} 出了 {round[player]}");
        }
    }

    private bool HasNextRound()
    {
        return _currentRound < TotalRound;
    }

    private Player DetermineRoundWinner(IDictionary<Player, Card?> round)
    {
        if (round == null) throw new ArgumentNullException(nameof(round));
        
        var maxCard = round.Max(kv => kv.Value);
        
        var winner = round.First(kv => kv.Value.Equals(maxCard)).Key;

        return winner;
    }

    private void OnRoundEnd(IDictionary<Player, Card?> round)
    {
        var winner = DetermineRoundWinner(round);
        winner.GainOnePoint();
        Console.WriteLine($"{winner.Name} 獲得該回合的勝利！ 目前:{winner.Point}分");
        _currentRound++;
    }
    
    private List<Player> GetGameWinner()
    {
        var maxPoint = Players.Max(p => p.Point);
        return Players.Where(p => p.Point == maxPoint).ToList();
    }
    
}