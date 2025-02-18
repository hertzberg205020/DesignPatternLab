namespace RpgGame.Models.GameComponent.IO;

public class ConsoleGameIO : IGameIO
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void PrintRoleStatus(Role role)
    {
        var status =
            $"輪到 {role.Name} (HP: {role.HealthPoint}, MP: {role.MagicPoint}, STR: {role.Strength}, State: {role.State?.ToString() ?? "正常"})。";
        WriteLine(status);
    }
}
