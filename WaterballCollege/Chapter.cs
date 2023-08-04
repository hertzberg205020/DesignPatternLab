namespace WaterballCollege;

public class Chapter
{
    private string _name;
    private int _number;
    private readonly ICollection<Mission> _missions;

    public string Name
    {
        get => _name;
        set => _name = ValidationUtils.LengthShouldBetween(value, 1, 30);
    }

    public int Number
    {
        get => _number;
        set => _number = ValidationUtils.ShouldBePositive(value);
    }

    public ICollection<Mission> Missions
    {
        get => _missions;
        init => _missions = ValidationUtils.RequiredNonNull(value);
    }

    public Chapter(string name, int number, ICollection<Mission> missions)
    {
        Name = name;
        Number = number;
        Missions = missions;
    }

    public Mission GetFirstMission() => Missions.First();
}