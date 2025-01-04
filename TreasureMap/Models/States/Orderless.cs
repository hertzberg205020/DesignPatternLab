using TreasureMap.Models.MapComponents;
using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Orderless : State
{
    public Orderless(Role role)
        : base(role, "混亂") { }

    public override int LeftRounds { get; set; } = 3;

    public override void PerformAction()
    {
        if (Role == null)
        {
            throw new InvalidOperationException("Role is null.");
        }

        if (Role is Monster)
        {
            base.Move();
        }

        // 隨機取得以下其中一種效果：
        var randomOpt = Random.Shared.Next(0, 2);
        Coordinate? newCoordination;

        var direction = randomOpt switch
        {
            0
                =>
                // 只能進行上下移動
                DirectionExtensions.GetMoveDirection(Direction.Up, Direction.Down),
            1
                =>
                // 只能進行左右移動
                DirectionExtensions.GetMoveDirection(Direction.Left, Direction.Right),
            _ => throw new InvalidOperationException("Invalid random option.")
        };

        newCoordination = Role.Position?.GetNextCoordinatesBasedOnDirection(direction);
        var map = Role.GameMap;

        ArgumentNullException.ThrowIfNull(newCoordination);

        map?.MoveRole(Role, newCoordination);
    }
}
