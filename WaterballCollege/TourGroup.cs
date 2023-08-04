namespace WaterballCollege;

public class TourGroup
{
    private readonly int _number;
    private readonly ICollection<Adventurer> _adventurers;

    public int Number   
    {
        get => _number;
        init => _number = ValidationUtils.ShouldBePositive(value);
    }

    public ICollection<Adventurer> Adventurers  
    {
        get => _adventurers;
        init {
            _adventurers = ValidationUtils.RequiredNonNull(value);
            foreach (var adventurer in _adventurers)
            {
                adventurer.TourGroup = this;
            }
        }
    }

    public TourGroup(int number, ICollection<Adventurer> adventurers)
    {
        Number = number;
        Adventurers = adventurers;
    }

    public void Add(Adventurer adventurer)
    {
        // 雙向關聯
        Adventurers.Add(adventurer);
        adventurer.TourGroup = this;
    }
}