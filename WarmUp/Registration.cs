namespace WarmUp;

public class Registration
{
    public Student Student { init; get; }

    public School School { init; get; }
    
    public int EntranceScore { init; get; }

    public Registration(Student student, School school, int entranceScore)
    {
        Student = student;
        School = school;
        EntranceScore = entranceScore;
    }
}