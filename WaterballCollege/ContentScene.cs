namespace WaterballCollege;

public class ContentScene: Scene
{
    public ContentScene(string name, int number, int expAward) : base(name, number, expAward)
    {
    }

    public override int CalculateExpAward() => (int)(ExpAward * 1.1);

}