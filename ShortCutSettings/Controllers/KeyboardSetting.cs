using System.Text.RegularExpressions;
using ShortCutSettings.Commands;
using ShortCutSettings.Receiver;

namespace ShortCutSettings.Controllers;

public class KeyboardSetting
{
    private readonly Dictionary<char, ICommand> _buttons = new Dictionary<char, ICommand>();

    private readonly MainController _mainController;
    
    private Stack<ICommand> UndoStack => _mainController.CommandUndoStack;
    
    private Stack<ICommand> RedoStack => _mainController.CommandRedoStack;
    
    public KeyboardSetting(MainController mainController)
    {
        _mainController = mainController;
    }
    
    public void SetButton(char button, ICommand command)
    {
        // use regex to check if the button is a letter [a-zA-Z]
        if (!Regex.IsMatch(button.ToString(), @"^[a-zA-Z]$"))
        {
            throw new ArgumentException("Button must be a letter");
        }
        
        _buttons[button] = command;
    }
    
    public void ShowCommands()
    {
        foreach (var button in _buttons)
        {
            Console.Write($"{button.Key}: {button.Value} \t");
        }
        Console.WriteLine();
    }
    
    public void PressButton(char button)
    {
        if (_buttons.TryGetValue(button, out var command))
        {
            command.Execute();
            UndoStack.Push(command);
            RedoStack.Clear();
        }
        else
        {
            Console.WriteLine("Invalid button");
        }
    }
    
    public void Undo()
    {
        if (UndoStack.TryPop(out var command))
        {
            command.Undo();
            RedoStack.Push(command);
        }
        else
        {
            Console.WriteLine("Nothing to undo");
        }
    }
    
    public void Redo()
    {
        if (RedoStack.TryPop(out var command))
        {
            command.Execute();
            UndoStack.Push(command);
        }
        else
        {
            Console.WriteLine("Nothing to redo");
        }
    }
}