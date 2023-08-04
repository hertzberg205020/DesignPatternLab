namespace WaterballCollege;

public class Challenge
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

    public Challenge(string name, int number)
    {
        Name = name;
        Number = number;
    }
}