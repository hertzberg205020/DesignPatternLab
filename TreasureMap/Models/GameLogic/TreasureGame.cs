using TreasureMap.Models.MapComponents;
using TreasureMap.Models.Roles;

namespace TreasureMap.Models.GameLogic;

public class TreasureGame
{
    public GameMap GameMap { get; init; }

    private GameState _currentState = GameState.InProgress;

    public TreasureGame(GameMap gameMap)
    {
        GameMap = gameMap;
        GameMap.Game = this;
    }

    public void Run()
    {
        Init();
        ExecuteRound();
        OnGameEnd();
    }

    /// <summary>
    /// 一開始生成地圖，隨機生成特定數量、位置的怪物和寶藏，
    /// 寶藏的內容物依照各內容物的生成機率隨機指定，
    /// 然後生成主角的位置以及隨機決定主角初始面向的方向。
    /// </summary>
    public void Init()
    {
        GameMap.RandomlyDistributeObjects();
        GameMap.RandomlyDecideCharacterDirection();
        _currentState = GameState.InProgress;
    }

    public void Render()
    {
        var mapRepresentation = GameMap.GetMapRepresentation();

        for (int y = 0; y < GameMap.Height; y++)
        {
            DrawHorizontalLine(GameMap.Width);
            DrawVerticalLines(mapRepresentation, GameMap.Width, y);
        }
        DrawHorizontalLine(GameMap.Width); // Draw the bottom line
    }

    /// <summary>
    /// 遊戲進行的主要迴圈，每一回合執行一次。
    /// </summary>
    public void ExecuteRound()
    {
        var round = 1;
        while (_currentState == GameState.InProgress)
        {
            Console.WriteLine($"Round {round++}");
            Render();
            ProcessCharacterTurns();
            ProcessMonsterTurns();
        }
    }

    public void OnGameEnd()
    {
        Console.WriteLine(
            _currentState == GameState.CharacterWin ? "Character wins!" : "Monsters win!"
        );
    }

    private void DrawHorizontalLine(int width)
    {
        for (int i = 0; i < width; i++)
        {
            Console.Write("+---");
        }
        Console.WriteLine("+");
    }

    private void DrawVerticalLines(char[,] map, int width, int row)
    {
        for (int i = 0; i < width; i++)
        {
            Console.Write($"| {map[row, i]} ");
        }
        Console.WriteLine("|");
    }

    private void ProcessRoleTurns()
    {
        foreach (var role in GameMap.Roles.ToList()) // 使用 ToList() 來避免集合修改的問題
        {
            if (role is Monster)
                Console.WriteLine("Monster's turn:");
            role.ExecuteTurn();
            if (_currentState != GameState.InProgress)
            {
                return;
            }
        }
    }

    private void ProcessCharacterTurns()
    {
        foreach (var character in GameMap.Characters.ToList()) // 使用 ToList() 來避免集合修改的問題
        {
            character.ExecuteTurn();
            if (_currentState != GameState.InProgress)
            {
                return;
            }
        }
    }

    private void ProcessMonsterTurns()
    {
        foreach (var monster in GameMap.Monsters.ToList()) // 使用 ToList() 來避免集合修改的問題
        {
            Console.WriteLine("Monster's turn:");
            monster.ExecuteTurn();
            if (_currentState != GameState.InProgress)
            {
                return;
            }
        }
    }

    internal void UpdateMapState(int characterCount, int monsterCount)
    {
        if (characterCount == 0)
        {
            _currentState = GameState.MonsterWin;
        }
        else if (monsterCount == 0)
        {
            _currentState = GameState.CharacterWin;
        }
    }
}
