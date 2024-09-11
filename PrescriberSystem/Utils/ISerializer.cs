namespace PrescriberSystem.Utils;

public interface ISerializer<T>
{
    void Serialize(T obj, string filePath);
}
