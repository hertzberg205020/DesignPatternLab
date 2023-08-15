namespace WarmUp;

public class Teacher: Employee
{
    private readonly ICollection<Student> _students = new HashSet<Student>();

    public School School { get; set; }

    public Teacher(ICollection<Student> students)
    {
        foreach (var student in students)
        {
            教學相長(student);
        }
    }

    /// <summary>
    /// 建立學生與教師的教學相長關聯
    /// </summary>
    /// <param name="student"></param>
    public void 教學相長(Student student)
    {
        _students.Add(student);
        student.AddTeachers(this);
    }
    
    public void Teach(Handout handout)
    {
        Console.WriteLine($"【講師】正在講解 {handout.Name} 的內容...");
    }
}