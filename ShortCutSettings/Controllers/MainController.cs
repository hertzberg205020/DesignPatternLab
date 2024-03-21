﻿using ShortCutSettings.Commands;
using ShortCutSettings.Receiver;

namespace ShortCutSettings.Controllers;

public class MainController
{
    public KeyboardSetting CurSetting { get; set; }

    public Stack<KeyboardSetting> KeyboardSettingUndoStack { get; } = new Stack<KeyboardSetting>();

    public Stack<KeyboardSetting> KeyboardSettingRedoStack { get; } = new Stack<KeyboardSetting>();
    
    public readonly Stack<ICommand> CommandUndoStack = new();
    
    public readonly Stack<ICommand> CommandRedoStack = new();

    private Dictionary<char, ICommand> CommandMapper { get; init; }

    public MainController()
    {
        CurSetting = new KeyboardSetting(this);
        CommandMapper = new Dictionary<char, ICommand>();
        
        var tank = new Tank();
        var telecom = new Telecom();
        CommandMapper.Add('0', new MoveForwardCommand(tank));
        CommandMapper.Add('1', new MoveBackwardCommand(tank));
        CommandMapper.Add('2', new ConnectCommand(telecom));
        CommandMapper.Add('3', new DisconnectCommand(telecom));
        CommandMapper.Add('4', new ResetMainControlKeyboardCommand(this));
    }
    
    public void Run()
    {
        while (true)
        {
            CurSetting.ShowCommands();
            Console.Write($"(1) 快捷鍵設置 (2) Undo (3) Redo (字母) 按下按鍵: ");
            var key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            
            switch (key)
            {
                case '1': 
                    SetCommand();
                    break;
                case '2':
                    Undo();
                    break;
                case '3':
                    Redo();
                    break;
                default:
                    PressKey(key);
                    break;
            }
        }
    }

    private void SetCommand()
    {
        Console.Write("key: ");
        var key = Console.ReadKey().KeyChar;
        Console.WriteLine();
        Console.WriteLine($"要將哪一道指令設置到快捷鍵 {key} 上: ");
        ShowCommands();
        var commandChoice = Console.ReadKey().KeyChar;
        Console.WriteLine();
        ICommand command = commandChoice switch
        {
            '0' => CommandMapper['0'],
            '1' => CommandMapper['1'],
            '2' => CommandMapper['2'],
            '3' => CommandMapper['3'],
            '4' => CommandMapper['4'],
            _ => throw new ArgumentOutOfRangeException()
        };
        SetCommand(key, command);
    }

    private void ShowCommands()
    {
        Console.WriteLine("(0) MoveTankForward");
        Console.WriteLine("(1) MoveTankBackward");
        Console.WriteLine("(2) ConnectTelecom");
        Console.WriteLine("(3) DisconnectTelecom");
        Console.WriteLine("(4) ResetMainControlKeyboard");
    }

    public void SetCommand(char key, ICommand command)
    {
        // (1) 快捷鍵設置 (2) Undo (3) Redo (字母) 按下按鍵:
        CurSetting.SetButton(key, command);
    }
    
    public void PressKey(char key)
    {
        CurSetting.PressButton(key);
    }
    
    public void Undo()
    {
        CurSetting.Undo();
    }
    
    public void Redo()
    {
        CurSetting.Redo();
    }
    
    public void ResetKeyboardSetting()
    {
        KeyboardSettingUndoStack.Push(CurSetting);
        CurSetting = new KeyboardSetting(this);
        KeyboardSettingRedoStack.Clear();
    }
    
    public void RestoreKeyboardSetting()
    {
        if (KeyboardSettingUndoStack.TryPop(out var setting))
        {
            CurSetting = setting;
            KeyboardSettingRedoStack.Push(CurSetting);
        }
    }
}