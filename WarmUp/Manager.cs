namespace WarmUp;

public class Manager: Employee
{
    public ICollection<Employee> Subordinates { init; get; } = new HashSet<Employee>();
    
    public Manager() : base()
    {
    }
    
    public void Mange(Employee employee)
    {
        Subordinates.Add(employee);
        employee.Manager = this;
    }
}