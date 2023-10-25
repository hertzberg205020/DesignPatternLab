namespace CardGameFramework.Models.Commons;

public interface ICard
{
    Enum Category { get; }
    Enum Value { get; }
}