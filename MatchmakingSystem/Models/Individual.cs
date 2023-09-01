namespace MatchmakingSystem.Models;

public class Individual
{
    private int _id;
    
    public int Id
    {
        get => _id;
        
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Id must be greater than 0.");
            }
            _id = value;
        }
    }

    private string _gender;

    public string Gender
    {
        get => _gender;
        set
        {
            if (string.Equals(value, "Male", StringComparison.OrdinalIgnoreCase))
            {
                _gender = "Male";
            }
            else if (string.Equals(value, "Female", StringComparison.OrdinalIgnoreCase))
            {
                _gender = "Female";
            }
            else
            {
                throw new AggregateException("Gender must be either 'Male' or 'Female'.");
            }
        }
    }

    private int _age;

    public int Age
    {
        get => _age;
        set
        {
            if (value < 18)
            {
                throw new ArgumentException("Age must be greater than 18.");
            }
            _age = value;
        }
    }

    private string _intro;

    public string Intro
    {
        get => _intro;
        set
        {
            if (value is null)
            {
                throw new ArgumentNullException("Intro must not be null.");
            }
            
            if (value.Length > 200)
            {
                throw new ArgumentException("Intro must be less than 200 characters.");
            }
            
            _intro = value;
        }
    }

    public readonly Habits habits = new();

    public string Habits => habits.ToString();

    public Coord Location { get; set; }

    public Individual(int id, string gender, int age, string intro, Coord location, params string[] habits)
    {
        Id = id;
        Gender = gender;
        Age = age;
        Intro = intro;
        Location = location;
        foreach (var h in habits)
        {
            this.habits.AddHabit(h);
        }
    }
}