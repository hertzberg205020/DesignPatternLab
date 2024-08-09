namespace TreasureMap.Models.MapComponents;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static Direction GetMoveDirection(params Direction[] directions)
    {
        while (true)
        {
            Console.WriteLine("Choose the direction to move: ");

            for (var i = 0; i < directions.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {directions[i]}");
            }

            var res = Console.ReadLine();

            if (
                !string.IsNullOrWhiteSpace(res)
                && int.TryParse(res, out var choice)
                && choice > 0
                && choice <= directions.Length
            )
            {
                return directions[choice - 1];
            }

            Console.WriteLine("Invalid choice, please select a valid direction.");
        }
    }
}
