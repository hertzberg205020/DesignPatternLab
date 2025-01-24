namespace RpgGame;

public class FileGameIO : IGameIO
{
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    public FileGameIO(string inputPath, string outputPath)
    {
        _reader = new StreamReader(inputPath);
        _writer = new StreamWriter(outputPath, false);
    }

    public string ReadLine()
    {
        return _reader.ReadLine() ?? string.Empty;
    }

    public void WriteLine(string message)
    {
        _writer.WriteLine(message);
        _writer.Flush(); // 確保立即寫入
    }

    public void Write(string message)
    {
        _writer.Write(message);
        _writer.Flush();
    }

    public bool HasNextLine()
    {
        return !_reader.EndOfStream;
    }

    public void Dispose()
    {
        _reader.Dispose();
        _writer.Dispose();
    }
}
