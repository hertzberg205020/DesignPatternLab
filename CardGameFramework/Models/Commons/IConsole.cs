namespace CardGameFramework.Models.Commons;

public interface IConsole
{
    string? ReadLine();
    void WriteLine(string message);
}