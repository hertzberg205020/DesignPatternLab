using ShortCutSettings.Receiver;

namespace ShortCutSettings.Commands;

public class MoveForwardCommand: ICommand
{
    private Tank Tank {get; init;}
    
    public MoveForwardCommand(Tank tank)
    {
        Tank = tank;
    }
    
    public void Execute()
    {
        Tank.MoveForward();
    }

    public void Undo()
    {
        Tank.MoveBackward();
    }
    
    public override string ToString()
    {
        return "MoveTankForward";
    }
}