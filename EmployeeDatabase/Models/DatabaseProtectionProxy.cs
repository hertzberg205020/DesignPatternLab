namespace EmployeeDatabase.Models;

public class DatabaseProtectionProxy : IDatabase
{
    private readonly RealDatabase _realDatabase;

    public DatabaseProtectionProxy(RealDatabase realDatabase)
    {
        _realDatabase = realDatabase;
    }

    /// <summary>
    /// checks if the Environment variable is set to allow access to the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEmployee? GetEmployeeById(int id)
    {
        // EMPLOYEE__PASSWORD=1qaz2wsx
        if (Environment.GetEnvironmentVariable("EMPLOYEE__PASSWORD") == "1qaz2wsx")
        {
            return _realDatabase.GetEmployeeById(id);
        }

        throw new UnauthorizedAccessException("Access denied: Invalid password.");
    }
}
