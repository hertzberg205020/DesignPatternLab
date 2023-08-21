namespace ShowDown.Models;

public class ShowDownGame
{
    public readonly List<Player> Players = new List<Player>();

    private readonly Deck _deck = new Deck();
    
    private int _currentRound = 0;

    private const int MaxNumberOfCardsOnHand = 13;

    private int HumanPlayerCount { get; set; }

    private const int TotalRound = 13;
    
    public IDictionary<Player, Card?> RoundCardPlay { get; private set; }
    
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
    private void OnInit()
    {
        InvestigateHumanPlayerCount();
        CreatePlayers();
        NamingPlayers();
        _deck.Shuffle();
    }
    
    /// <summary>
    /// 抽牌階段
    /// </summary>
    private void OnCardDraw()
    {
        for (var i = 0; i < MaxNumberOfCardsOnHand; i++)
        {
            foreach (var player in Players)
            {
                _deck.DealCardToPlayer(player);
            }
        }
    }

    private void OnPlayTurns()
    {
        while (HasNextRound())
        {
            TakeATurn();
            ShowCards();
            DetermineRoundWinner();
            _currentRound++;
        }
    }

    private void ShowCards()
    {
        foreach (var player in RoundCardPlay.Keys)
        {
            Console.WriteLine($"{player.Name} 出了 {RoundCardPlay[player]}");
        }
    }

    private void TakeATurn()
    {
        InitializeRoundCardPlay();
        foreach (var player in Players)
        {
            player.MakeExchangeDecision(Players);
            var card = player.Show();
            RoundCardPlay[player] = card;
            player.UpdateExchangeState();
        }
    }

    private void InitializeRoundCardPlay()
    {
        RoundCardPlay = new Dictionary<Player, Card?>(); 
        Players.ForEach(p => RoundCardPlay.Add(p, null));
    }
    
    private void OnGameEnd()
    {
        
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

    private void CreatePlayers()
    {
        for (var i = 0; i < HumanPlayerCount; i++)
        {
            Players.Add(new HumanPlayer());
        }
        
        for (var i = 0; i < 4 - HumanPlayerCount; i++)
        {
            Players.Add(new AiPlayer());
        }
    }
    
    private void InvestigateHumanPlayerCount()
    {
        Console.WriteLine("請輸入真實玩家人數：");
        var input = Console.ReadLine();
        if (int.TryParse(input, out var count) && count > 0)
        {
            HumanPlayerCount = count;
        }
        else
        {
            Console.WriteLine("輸入不正確，請再次輸入一個大於 0 的數字!");
            InvestigateHumanPlayerCount();
        }
    }

    private bool HasNextRound()
    {
        return _currentRound < TotalRound;
    }
    
    private Player DetermineRoundWinner()
    {
        if (RoundCardPlay == null) throw new ArgumentNullException(nameof(RoundCardPlay));

        // 根據牌的大小排序
        var maxCard = RoundCardPlay.Max(kv => kv.Value);
        
        // 找出出最大牌的玩家
        var winningPlayer = RoundCardPlay.First(kv => kv.Value.Equals(maxCard)).Key;

        return winningPlayer;
    }
    
    private Player GetGameWinner()
    {
        var maxPoint = Players.Max(p => p.Point);
        return Players.First(p => p.Point == maxPoint);
    }
    
    
}