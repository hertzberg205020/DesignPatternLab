﻿namespace ShortCutSettings.Commands;

public interface ICommand
{
    void Execute();
    void Undo();
}