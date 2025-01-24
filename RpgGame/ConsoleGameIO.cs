namespace RpgGame;

public class ConsoleGameIO : IGameIO
{
    public string ReadLine()
    {
        return Console.ReadLine() ?? string.Empty;
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void Write(string message)
    {
        Console.Write(message);
    }

    public bool HasNextLine()
    {
        return true; // Console 永遠可以讀取下一行
    }
}
