namespace WaterballCollege;

public class MissionCarryOn
{
    public enum State
    {
        COMPLETED,
        IN_PROGRESS,
    }

    public State CurrentState  { get; private set; } = State.IN_PROGRESS;

    public Mission Mission { get; init; }

    public Student Student { get; init; }
    
    public MissionCarryOn(Mission mission, Student student)
    {
        Mission = mission;
        Student = student;
    }
    
    public void Complete()
    {
        CurrentState = State.COMPLETED;

        Console.WriteLine($"【任務】學員 {Student.Account} 已成功完成任務 {Mission.Name}");
        
        Student.GainExp(Mission.CalculateExpAward());
    }
}