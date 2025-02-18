namespace RpgGame.Tests.Helpers;

public class TestGameIO : IGameIO
{
    private readonly Queue<string> _inputs;
    private readonly List<string> _outputs;

    public TestGameIO(IEnumerable<string> inputs)
    {
        _inputs = new Queue<string>(inputs);
        _outputs = new List<string>();
    }

    public string? ReadLine()
    {
        return _inputs.Count > 0 ? _inputs.Dequeue() : null;
    }

    public void WriteLine(string message)
    {
        _outputs.Add(message);
    }

    public void PrintRoleStatus(Role role)
    {
        var status =
            $"輪到 {role.Name} (HP: {role.HealthPoint}, MP: {role.MagicPoint}, STR: {role.Strength}, State: {role.State?.ToString() ?? "正常"})。";
        _outputs.Add(status);
    }

    // 用於測試驗證
    public IReadOnlyList<string> Outputs => _outputs;

    // read test input
    public void Input(string input)
    {
        _inputs.Enqueue(input);
    }
}
