using TreasureMap.Models.MapComponents;

namespace TreasureMap.Models.Roles;

public class Character : Role
{
    public Direction Direction { get; set; }

    public override int MaxHp { get; set; } = 300;

    public override int Hp { get; set; } = 300;

    public override string TakeDecision(params string[] options)
    {
        while (true)
        {
            Console.WriteLine("Choose the action to take in character turn: ");
            for (var i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            var res = Console.ReadLine();

            if (
                res != null
                && int.TryParse(res, out var choice)
                && choice > 0
                && choice <= options.Length
            )
            {
                return options[choice - 1];
            }

            Console.WriteLine("Invalid choice, please select a valid action.");
        }
    }

    public override char Symbol =>
        Direction switch
        {
            Direction.Up => '\u2191',
            Direction.Down => '\u2193',
            Direction.Left => '\u2190',
            Direction.Right => '\u2192',
            _ => throw new ArgumentException("Invalid direction.")
        };

    public override Direction MoveDirection()
    {
        var directionMap = new Dictionary<string, Direction>
        {
            { "1", Direction.Up },
            { "2", Direction.Down },
            { "3", Direction.Left },
            { "4", Direction.Right }
        };

        while (true)
        {
            Console.WriteLine("Choose the direction to move: ");
            Console.WriteLine("1. Up");
            Console.WriteLine("2. Down");
            Console.WriteLine("3. Left");
            Console.WriteLine("4. Right");

            var res = Console.ReadLine();

            if (res != null && directionMap.TryGetValue(res, out var direction))
            {
                return direction;
            }

            Console.WriteLine("Invalid choice, please select a valid direction.");
        }
    }
}
