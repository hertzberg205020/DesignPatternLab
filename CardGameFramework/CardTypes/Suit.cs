namespace CardGameFramework.CardTypes;

public sealed class Suit
{
    public static readonly Suit Heart = new Suit("Heart", 1);
    public static readonly Suit Diamond = new Suit("Diamond", 2);
    public static readonly Suit Club = new Suit("Club", 3);
    public static readonly Suit Spade = new Suit("Spade", 4);

    public string Name { get; private set; }
    public int Value { get; private set; }

    private Suit(string name, int value)
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
        if (obj is Suit other)
        {
            return this.Value == other.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
