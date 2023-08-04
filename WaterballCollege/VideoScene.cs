namespace WaterballCollege;

public class VideoScene: Scene
{
    public VideoScene(string name, int number, int expAward) : base(name, number, expAward)
    {
    }

    public override int CalculateExpAward() => (int)(ExpAward * 1.5);
    
}