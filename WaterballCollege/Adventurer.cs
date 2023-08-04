namespace WaterballCollege;

public class Adventurer
{
    private Student _student;
    private Journey _journey;
    private int _number;
    private TourGroup _tourGroup;

    public int Number
    {
        get => _number;
        set => _number = ValidationUtils.ShouldBePositive(value);
    }

    public Student Student
    {
        get => _student;
        set => _student = ValidationUtils.RequiredNonNull(value);
    }

    public Journey Journey
    {
        get => _journey;
        set => _journey = ValidationUtils.RequiredNonNull(value);
    }

    public TourGroup TourGroup 
    {
        get => _tourGroup;
        set => _tourGroup = ValidationUtils.RequiredNonNull(value);
    }

    public Adventurer(int number, Student student, Journey journey)
    {
        Number = number;
        Student = student;
        Journey = journey;
    }

    public void CarryOn(Mission mission)
    {
        Student.CarryOn(mission);
    }
}