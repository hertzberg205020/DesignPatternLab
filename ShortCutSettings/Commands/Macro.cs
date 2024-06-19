using System.Text;

namespace ShortCutSettings.Commands;

public class Macro : ICommand
{
    private readonly List<ICommand> _commands = new();

    public void Execute()
    {
        _commands.ForEach(command => command.Execute());
    }

    public void Undo()
    {
        // reverse the order of the commands
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
        }
    }

    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        return string.Join(" & ", _commands.Select(command => command.ToString()));
    }
}