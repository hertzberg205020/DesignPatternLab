namespace EmployeeDatabase.Models;

public interface IEmployee
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public ICollection<IEmployee> Subordinates { get; }
}
