namespace EmployeeDatabase.Models;

public interface IDatabase
{
    IEmployee? GetEmployeeById(int id);
}
