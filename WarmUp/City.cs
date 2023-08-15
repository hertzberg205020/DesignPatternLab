namespace WarmUp;

public class City
{
    public ICollection<Road> Roads { init; get; } = new HashSet<Road>();
    
    public City()
    {
    }
    
    public void Connect(City city)
    {
        Roads.Add(new Road(this, city));
        city.Roads.Add(new Road(city, this));
    }
}