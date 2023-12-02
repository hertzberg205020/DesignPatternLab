namespace CardGameFramework.Infra.Models;

public class RealConsole: IConsole
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}