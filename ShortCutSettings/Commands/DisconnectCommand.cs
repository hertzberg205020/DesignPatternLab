using ShortCutSettings.Receiver;

namespace ShortCutSettings.Commands;

public class DisconnectCommand: ICommand
{
    private Telecom Telecom {get; init;}
    
    public DisconnectCommand(Telecom telecom)
    {
        Telecom = telecom;
    }
    
    public void Execute()
    {
        Telecom.Disconnect();
    }

    public void Undo()
    {
        Telecom.Connect();
    }

    public override string ToString()
    {
        return "DisconnectTelecom";
    }
}