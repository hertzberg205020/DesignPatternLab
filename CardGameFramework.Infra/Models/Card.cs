namespace CardGameFramework.Infra.Models;

public abstract class Card<TCategory, TValue> : ICard
    where TCategory : Enum
    where TValue : Enum
{
    protected Card(TCategory category, TValue value)
    {
        Category = category;
        Value = value;
    }

    public Enum Category { get; }
    public Enum Value { get; }

    public override bool Equals(object? obj)
    {
        return obj is Card<TCategory, TValue> card && Equals(card);
    }

    private bool Equals(Card<TCategory, TValue> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        return EqualityComparer<TCategory>.Equals(Category, other.Category) &&
               EqualityComparer<TValue>.Equals(Value, other.Value);

        // return Category.Equals(other.Category) && Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Category, Value);
    }

    public override string ToString()
    {
        return $"{Category} {Value}";
    }
}