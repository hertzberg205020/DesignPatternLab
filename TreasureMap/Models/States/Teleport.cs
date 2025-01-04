using TreasureMap.Models.MapComponents;
using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Teleport : State
{
    public Teleport(Role role)
        : base(role, "瞬身") { }

    public override int LeftRounds { get; set; } = 1;

    public override void ExitState()
    {
        // Teleport to a random position
        if (Role is not { GameMap: not null })
            throw new InvalidOperationException("Role or GameMap is null.");

        Coordinate? coord;
        do
        {
            coord = GenerateRandomCoordinate(Role.GameMap.Width, Role.GameMap.Height);
        } while (Role.GameMap.IsPositionOccupied(coord));

        Console.WriteLine($"角色因為傳送狀態的效果進行傳送。");
        Role.GameMap.MoveRole(Role, coord);
        Role.GameMap.Game.Render();
    }

    private static Coordinate GenerateRandomCoordinate(int width, int height)
    {
        var x = Random.Shared.Next(0, width);
        var y = Random.Shared.Next(0, height);
        return new Coordinate(x, y);
    }
}
