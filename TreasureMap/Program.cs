using TreasureMap.Models.GameLogic;
using TreasureMap.Models.MapComponents;
using TreasureMap.Models.Roles;
using TreasureMap.Models.States;
using TreasureMap.Models.Treasures;

namespace TreasureMap;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        var obstacles = CreateObstacles(0);

        // var treasures = TreasureFactory.CreateTreasures(0);

        var treasures = new List<Treasure>
        {
            new HealingPotion()
        };

        var character = new Character();

        character.EnterState(new Poisoned(character));

        var characters = new List<Character> { character };

        var monsters = CreateMonsters(1);

        var gameMap = new GameMap(4, 4, characters, monsters, treasures, obstacles);
        var game = new TreasureGame(gameMap);
        game.Run();
    }

    private static List<Obstacle> CreateObstacles(int count)
    {
        if (count < 0)
        {
            throw new ArgumentException(
                "The count of obstacles should be greater than or equal to 0."
            );
        }

        var obstacles = new Obstacle[count];
        for (var i = 0; i < count; i++)
        {
            obstacles[i] = new Obstacle();
        }

        return obstacles.ToList();
    }

    private static List<Monster> CreateMonsters(int count)
    {
        if (count < 0)
        {
            throw new ArgumentException(
                "The count of monsters should be greater than or equal to 0."
            );
        }

        var monsters = new Monster[count];
        for (var i = 0; i < count; i++)
        {
            monsters[i] = new Monster();
        }

        return monsters.ToList();
    }
}
