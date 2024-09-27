using System.Text;
using EmployeeDatabase.Models;

namespace EmployeeDatabase;

class Program
{
    static void Main(string[] args)
    {
        var root = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(root, "Resources", "data.txt");
        var database = new DatabaseProtectionProxy(new RealDatabase(filePath));
        var employee = database.GetEmployeeById(2);
        if (employee != null)
        {
            PrintEmployee(employee);
        }
        else
        {
            Console.WriteLine("Employee not found");
        }
    }

    private static void PrintEmployee(IEmployee employee)
    {
        Console.WriteLine(employee);
        var stringBuilder = new StringBuilder("Subordinates:");
        foreach (var subordinate in employee.Subordinates)
        {
            stringBuilder.Append($"\n\t{subordinate}");
        }

        Console.WriteLine(stringBuilder);
    }
}
