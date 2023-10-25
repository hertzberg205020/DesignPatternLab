namespace CardGameFramework.CardTypes;

public sealed class Rank
{
    public static readonly Rank Ace = new Rank("Ace", 1);
    public static readonly Rank Two = new Rank("Two", 2);
    public static readonly Rank Three = new Rank("Three", 3);
    public static readonly Rank Four = new Rank("Four", 4);
    public static readonly Rank Five = new Rank("Five", 5);
    public static readonly Rank Six = new Rank("Six", 6);
    public static readonly Rank Seven = new Rank("Seven", 7);
    public static readonly Rank Eight = new Rank("Eight", 8);
    public static readonly Rank Nine = new Rank("Nine", 9);
    public static readonly Rank Ten = new Rank("Ten", 10);
    public static readonly Rank Jack = new Rank("Jack", 11);
    public static readonly Rank Queen = new Rank("Queen", 12);
    public static readonly Rank King = new Rank("King", 13);

    public string Name { get; private set; }
    public int Value { get; private set; }

    private Rank(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object obj)
    {
        if (obj is Rank other)
        {
            return this.Value == other.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    // 如果你需要像 Enum.GetValues() 的功能，你可以添加以下方法
    public static IEnumerable<Rank> GetAllRanks()
    {
        yield return Ace;
        yield return Two;
        yield return Three;
        yield return Four;
        yield return Five;
        yield return Six;
        yield return Seven;
        yield return Eight;
        yield return Nine;
        yield return Ten;
        yield return Jack;
        yield return Queen;
        yield return King;
    }
}
