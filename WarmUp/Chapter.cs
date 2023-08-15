namespace WarmUp;

public class Chapter
{
    public ICollection<Mission> Missions { get; } = new List<Mission>();

    public Journey Journey { get; init; }
    
    public Chapter(Journey journey)
    {
        Journey = journey;
    }
    
    public void AddMission(Mission mission)
    {
        Missions.Add(mission);
    }
}