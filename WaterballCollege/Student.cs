namespace WaterballCollege;

public class Student
{
    private static readonly LevelSheet LEVEL_SHEET = new();

    public required string Account { get; init; }


    public required string Password { get; init; }

    public int Level { get; set; } = 1;

    public int Exp { get; set; } = 0;


    public Student()
    {
    }

    public Student(string account, string password)
    {
        Account = account;
        Password = password;
    }

    public void GainExp(int exp)
    { 
        Exp += exp;

        var newLevel = LEVEL_SHEET.Query(Exp);

        int levelUp = newLevel - Level;

        Console.WriteLine($"學員 {Account} 獲得經驗值 {exp}");

        foreach(var i in Enumerable.Range(1, levelUp))
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level++;
        Console.WriteLine($"【升等】 學員 {Account} 等級提升至 {Level}");
    }
}
