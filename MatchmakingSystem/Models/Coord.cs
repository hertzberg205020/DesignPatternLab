namespace MatchmakingSystem.Models;

public class Coord
{
    public double XAxis { get; }
    
    public double YAxis { get; }

    public Coord(double xAxis, double yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }
    
    public double DistanceTo(Coord other)
    {
        var xDiff = XAxis - other.XAxis;
        var yDiff = YAxis - other.YAxis;
        return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
    }
}