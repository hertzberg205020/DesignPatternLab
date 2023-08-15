namespace WarmUp;

public class Student
{
    private readonly ICollection<Teacher> _teachers = new HashSet<Teacher>();
    
    public Registration Registrations { get; set; }

    public Student()
    {
        
    }
    public void AddTeachers(Teacher teacher)
    {
        _teachers.Add(teacher);
    }
}