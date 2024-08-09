namespace TreasureMap.Models.MapComponents;

public class Coordinate
{
    public int XAxis { get; init; }

    public int YAxis { get; init; }

    public Coordinate(int x, int y)
    {
        XAxis = x;
        YAxis = y;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var coordinate = (Coordinate)obj;
        return XAxis == coordinate.XAxis && YAxis == coordinate.YAxis;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(XAxis, YAxis);
    }

    public override string ToString()
    {
        return $"({XAxis}, {YAxis})";
    }

    public double DistanceTo(Coordinate coordinate)
    {
        return Math.Sqrt(
            Math.Pow(XAxis - coordinate.XAxis, 2) + Math.Pow(YAxis - coordinate.YAxis, 2)
        );
    }

    public Coordinate GetNextCoordinatesBasedOnDirection(Direction direction)
    {
        if (!Enum.IsDefined(typeof(Direction), direction))
        {
            throw new ArgumentException("Invalid direction");
        }

        return direction switch
        {
            Direction.Up => new Coordinate(XAxis, YAxis - 1),
            Direction.Down => new Coordinate(XAxis, YAxis + 1),
            Direction.Left => new Coordinate(XAxis - 1, YAxis),
            Direction.Right => new Coordinate(XAxis + 1, YAxis),
            _ => throw new ArgumentException("Invalid direction")
        };
    }

    public bool IsNextTo(Coordinate coordinate)
    {
        var dx = XAxis - coordinate.XAxis;
        var dy = YAxis - coordinate.YAxis;

        // Check if one difference is 1 and the other is 0
        return (Math.Abs(dx) == 1 && dy == 0) || (dx == 0 && Math.Abs(dy) == 1);
    }
}
