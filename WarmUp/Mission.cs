namespace WarmUp;

public class Mission
{
    public Chapter Chapter { get; init; }

    public ICollection<Scene> Scenes { get; } = new List<Scene>();

    public ICollection<Challenge> Challenges { get; init; } = new List<Challenge>();
    
    public Mission(Chapter chapter)
    {
        Chapter = chapter;
    }
    
    public void AddScene(Scene scene)
    {
        Scenes.Add(scene);
    }
    
    public void AddChallenge(Challenge challenge)
    {
        Challenges.Add(challenge);
    }
    
}