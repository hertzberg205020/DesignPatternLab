namespace EmployeeDatabase.Models;

/// <summary>
/// implements the virtual proxy pattern
/// to defer the creation of the real employee object until it is actually needed with lazy initialization.
/// </summary>
public class EmployeeVirtualProxy : IEmployee
{
    private readonly RealDatabase _database;
    private readonly int _id;
    private readonly Lazy<ICollection<IEmployee>> _subordinates;
    private RealEmployee? _employee;

    public EmployeeVirtualProxy(RealDatabase database, int id)
    {
        _database = database;
        _id = id;
        _employee = GetRealEmployee();
        _subordinates = new Lazy<ICollection<IEmployee>>(() =>
        {
            var realEmployee = GetRealEmployee();

            return realEmployee
                .SubordinateIds.Select(subordinateId => new EmployeeVirtualProxy(
                    _database,
                    subordinateId
                ))
                .ToList<IEmployee>();
        });
    }

    public int Id => GetRealEmployee().Id;
    public string Name => GetRealEmployee().Name;
    public int Age => GetRealEmployee().Age;

    public ICollection<IEmployee> Subordinates => _subordinates.Value;

    private RealEmployee GetRealEmployee()
    {
        if (_employee is null)
        {
            var employee = _database.GetRealEmployeeById(_id);
            if (employee is not null)
            {
                _employee = employee;
            }
            else
            {
                throw new InvalidOperationException($"Employee with ID {_id} not found.");
            }
        }

        return _employee;
    }

    public override string ToString()
    {
        if (_employee != null)
        {
            return _employee.ToString() ?? string.Empty;
        }

        throw new InvalidOperationException("Employee not found.");
    }
}
