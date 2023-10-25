﻿using CardGameFramework.Models.Commons;

namespace CardGameFramework.Models.ShowDownGame;

public class ShowDownGame: CardGame<PokerCard>
{
    public const int NumOfRounds = 13;
    private readonly List<TurnMove> _turnMoves = new();
    
    public ShowDownGame(Deck<PokerCard> deck, List<CardPlayer<PokerCard>> players) 
        : base(deck, players)
    {
    }

    protected override void OnGameRoundsStart()
    {
        for (int i = 0; i < NumOfRounds; i++)
        {
            // 使用 OfType<T>()，僅會返回可以被成功轉型的物件
            Players.OfType<IShowDownGamePlayer>().ToList().ForEach(TakeTurn);
            ShowDown();
            _turnMoves.Clear();
        }
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

    protected override void OnGameEnded()
    {
        var winner = Players.OfType<IShowDownGamePlayer>()
            .OrderByDescending(player => player.Points)
            .First();
        Console.WriteLine($"{winner.Name} wins the game.");
    }
    
}