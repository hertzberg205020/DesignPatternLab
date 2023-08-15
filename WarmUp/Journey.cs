namespace WarmUp;

public class Journey
{
    public ICollection<Chapter> Chapters { get; } = new List<Chapter>();

    public void AddChapter(Chapter chapter)
    {
        Chapters.Add(chapter);
    }
}