namespace WaterballCollege;

public class Mission
{
    private string _name;
    private int _number;
    private ICollection<Scene> _scenes;
    private Challenge _challenge;
    
    public string Name
    {
        get => _name;
        set => _name = ValidationUtils.LengthShouldBetween(value, 1, 30);
    }

    public int Number
    {
        get => _number;
        set => _number = ValidationUtils.ShouldBePositive(value);
    }

    public ICollection<Scene> Scenes
    {
        get => _scenes;
        set => _scenes = ValidationUtils.RequiredNonNull(value);
    }

    public Challenge Challenge
    {
        get => _challenge;
        set => _challenge = ValidationUtils.RequiredNonNull(value);
    }


    public Mission(string name, int number, ICollection<Scene> scenes, Challenge challenge)
    {
        Name = name;
        Number = number;
        Scenes = scenes;
        Challenge = challenge;
        Challenge = challenge;
    }
    
    /// <summary>
    /// 所含有Scene的經驗值總和
    /// </summary>
    /// <returns></returns>
    public int CalculateExpAward()
    {
        return Scenes.Sum(scene => scene.CalculateExpAward());
    }
}