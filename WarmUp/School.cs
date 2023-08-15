namespace WarmUp;

public class School
{
    public ICollection<Teacher> Teachers { init; get; } = new HashSet<Teacher>();

    public ICollection<Classroom> Classrooms { init; get; } = new HashSet<Classroom>();
    
    public ICollection<Registration> Registrations { init; get; } = new HashSet<Registration>();

    
    public School()
    {
    }

    public void Employee(Teacher teacher)
    {
        Teachers.Add(teacher);
        teacher.School = this;
    }
    
    public void AddClassroom(Classroom classroom)
    {
        Classrooms.Add(classroom);
    }

    public void Register(Student student, int entranceScore)
    {
        var registration = new Registration(student, this, entranceScore);
        this.Registrations.Add(registration);
        student.Registrations = registration;
    }
    
}