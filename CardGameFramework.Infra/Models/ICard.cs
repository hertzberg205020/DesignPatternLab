namespace CardGameFramework.Infra.Models;

public interface ICard
{
    Enum Category { get; }
    Enum Value { get; }
}