namespace RpgGame;

public interface IGameIO
{
    string ReadLine();
    void WriteLine(string message);
    void Write(string message);
    bool HasNextLine();
}
