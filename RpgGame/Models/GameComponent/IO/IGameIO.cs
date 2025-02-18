namespace RpgGame.Models.GameComponent.IO;

public interface IGameIO
{
    /// <summary>
    /// 讀取一行輸入
    /// </summary>
    string? ReadLine();

    /// <summary>
    /// 輸出一行訊息
    /// </summary>
    void WriteLine(string message);

    /// <summary>
    /// 輸出角色狀態
    /// </summary>
    void PrintRoleStatus(Role role);
}
