namespace WaterballCollege;

public  class Journey
{
    private string _name;
    private string _description;
    private decimal _price;
    private readonly ICollection<Chapter> _chapters;
    private readonly ICollection<Adventurer> _adventurers;
    private readonly IList<TourGroup> _tourGroups;

    public Journey(string name,
        string description,
        decimal price,
        ICollection<Chapter> chapters,
        ICollection<Adventurer> adventurers,
        IList<TourGroup> tourGroups
        )
    {
        Name = name;
        Description = description;
        Price = price;
        Chapters = chapters;
        Adventurers = adventurers;
        TourGroups = tourGroups;
    }

    public string Name
    {
        get => _name;
        set => _name = ValidationUtils.LengthShouldBetween(value, 1, 30);
    }

    public string Description
    {
        get => _description;
        set => _description = ValidationUtils.LengthShouldBetween(value, 1, 300);
    }

    public decimal Price
    {
        get => _price;
        set => _price = ValidationUtils.ShouldBeLargerThan(value, 1);
    }

    public ICollection<Chapter> Chapters
    {
        get => _chapters;
        init => _chapters = ValidationUtils.RequiredNonNull(value);
    }

    public ICollection<Adventurer> Adventurers
    {
        get => _adventurers;
        init => _adventurers = ValidationUtils.RequiredNonNull(value);
    }

    public IList<TourGroup> TourGroups
    {
        get => _tourGroups;
        init => _tourGroups = ValidationUtils.RequiredNonNull(value);
    }

    public Adventurer Join(Student student)
    {
        var number = Adventurers.Count + 1;
        
        // 建立與冒險者之間的雙向關聯
        var adventurer = new Adventurer(number, student, this);
        Adventurers.Add(adventurer);
        student.Adventurers.Add(adventurer);
        
        // 開始第一項任務
        var firstMission = GetFirstMission();
        adventurer.CarryOn(firstMission);
        
        // 匹配旅團
        var tourGroup = matchTourGroup(adventurer);
        TourGroups.Add(tourGroup);
        Console.WriteLine($"【旅程】冒險者 {student.Account} 加入旅程 ${Name} --> 匹配至旅團 {tourGroup.Number}。\n");
        
        return adventurer;
    }

    /// <summary>
    ///  匹配算法：將新冒險者匹配至某個旅團中
    /// </summary>
    /// <param name="adventurer"></param>
    /// <returns></returns>
    private TourGroup matchTourGroup(Adventurer adventurer)
    {
        
        if (TourGroups.Count > 0)
        {
            var rndNum = new Random().Next(TourGroups.Count);
            var tourGroup = TourGroups[rndNum];
        }

        return new TourGroup(1, new List<Adventurer> {adventurer});
    }

    private Mission GetFirstMission()
    {
        return Chapters.First().GetFirstMission();
    }
}
