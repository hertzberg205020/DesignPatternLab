namespace CardGameFramework.Infra.Models;

public class RealConsole: IConsole
{
    public RealConsole()
    {
        Console.Out.NewLine = "\n";
    }
    
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
    
    public void SetOut(TextWriter textWriter)
    {
        Console.SetOut(textWriter);
    }
    
    public void SetIn(TextReader textReader)
    {
        Console.SetIn(textReader);
    }
}