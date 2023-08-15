namespace WarmUp;

public class Road
{
    public City City1 { get; set; }
    public City City2 { get; set; }

    public Road(City city1, City city2)
    {
        City1 = city1;
        City2 = city2;
    }
}