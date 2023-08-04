namespace WaterballCollege;

public class Student
{
    private static readonly LevelSheet LEVEL_SHEET = new();
    private readonly ICollection<MissionCarryOn> _missionCarryOns;
    private readonly string _account;
    private readonly string _password;
    private int _level = 1;
    private int _exp = 0;
    private readonly ICollection<Adventurer> _adventurers;

    public string Account
    {
        get => _account;
        init => _account = value;
    }

    public string Password
    {
        get => _password;
        init => _password = value;
    }

    public int Level
    {
        get => _level;
        private set => _level = value;
    }

    public int Exp
    {
        get => _exp;
        private set => _exp = value;
    }

    public ICollection<MissionCarryOn> MissionCarryOns
    {
        get => _missionCarryOns;
        init => _missionCarryOns = ValidationUtils.RequiredNonNull(value);
    }

    public ICollection<Adventurer> Adventurers
    {
        get => _adventurers;
        init => _adventurers = ValidationUtils.RequiredNonNull(value);
    }

    public Student(
        string account,
        string password,
        ICollection<MissionCarryOn> missionCarryOns,
        ICollection<Adventurer> adventurers
        )
    {
        Account = account;
        Password = password;
        MissionCarryOns = missionCarryOns;
        Adventurers = adventurers;
    }

    public MissionCarryOn CarryOn(Mission mission)
    {
        Console.WriteLine($"【任務】學員 {Account} 開始新任務：{mission.Name}\n");
        MissionCarryOn missionCarryOn = new(mission, this);
        MissionCarryOns.Add(missionCarryOn);  // 單向關聯
        return missionCarryOn;
    }

    public void GainExp(int exp)
    { 
        Exp += exp;

        var newLevel = LEVEL_SHEET.Query(Exp);

        int levelUp = newLevel - Level;

        Console.WriteLine($"學員 {Account} 獲得經驗值 {exp}");

        foreach(var _ in Enumerable.Range(1, levelUp))
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
