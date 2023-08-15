namespace WarmUp;

public class Classmate
{
    public ICollection<Classmate> Friends { init; get; } = new HashSet<Classmate>();
    
    
    public Classmate()
    {
    }
    
    public void MakeFriend(Classmate classmate)
    {
        Friends.Add(classmate);
        classmate.Friends.Add(this);
    }
}