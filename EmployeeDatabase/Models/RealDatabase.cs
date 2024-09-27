namespace EmployeeDatabase.Models;

public class RealDatabase : IDatabase
{
    private readonly string _filePath;

    public RealDatabase(string filePath)
    {
        _filePath = filePath;
    }

    internal RealEmployee? GetRealEmployeeById(int id)
    {
        var lines = File.ReadLines(_filePath).Skip(1); // 跳過標題行

        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            if (parts.Length >= 3 && int.Parse(parts[0]) == id)
            {
                return new RealEmployee
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Age = int.Parse(parts[2]),
                    SubordinateIds =
                        parts.Length > 3
                            ? parts[3].Split(',').Select(int.Parse).ToList()
                            : new List<int>()
                };
            }
        }

        return null; // 如果找不到指定 ID 的人員
    }

    public IEmployee? GetEmployeeById(int id)
    {
        return new EmployeeVirtualProxy(this, id);
    }
}
