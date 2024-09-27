namespace EmployeeDatabase.Models;

public class RealEmployee : IEmployee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<int> SubordinateIds { get; set; } = new List<int>();

    public ICollection<IEmployee> Subordinates { get; set; } = new List<IEmployee>();

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Age: {Age}, SubordinateIds: [{string.Join(", ", SubordinateIds)}]";
    }
}
