namespace WaterballCollege;

public abstract class Scene
{
    private string _name;
    private int _number;

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

    protected int ExpAward { get; set; }

    protected Scene(string name, int number, int expAward)
    {
        Name = name;
        Number = number;
        ExpAward = expAward;
    }
    
    public abstract int CalculateExpAward();
}