using ShortCutSettings.Receiver;

namespace ShortCutSettings.Commands;

public class MoveBackwardCommand: ICommand
{
    private Tank Tank {get; init;}

    public MoveBackwardCommand(Tank tank)
    {
        Tank = tank;
    }
    
    public void Execute()
    {
        Tank.MoveBackward();
    }

    public void Undo()
    {
        Tank.MoveForward();
    }
}