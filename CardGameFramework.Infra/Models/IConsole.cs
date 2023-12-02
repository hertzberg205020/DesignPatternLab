namespace CardGameFramework.Infra.Models;

public interface IConsole
{
    string? ReadLine();
    void WriteLine(string message);
}