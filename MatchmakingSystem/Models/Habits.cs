namespace MatchmakingSystem.Models;

public class Habits
{
    public readonly HashSet<string> habits = new ();
    

    public void AddHabit(string habit)
    {
        if (habit == null)
        {
            throw new ArgumentNullException(nameof(habit));
        }

        if (habit.Length is < 1 or > 10)
        {
            throw new ArgumentException("Habit length should be between 1 and 10.", nameof(habit));
        }
        
        habits.Add(habit);
    }
    
    public int SimilarityTo(Habits other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var intersection = habits.Intersect(other.habits);
        return intersection.Count();
    }

    public override string ToString()
    {
        return string.Join(", ", habits);
    }
}