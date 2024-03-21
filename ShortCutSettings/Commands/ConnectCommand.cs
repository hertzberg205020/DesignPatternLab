using ShortCutSettings.Receiver;

namespace ShortCutSettings.Commands;

public class ConnectCommand: ICommand
{
    private Telecom Telecom {get; init;}
    
    public ConnectCommand(Telecom telecom)
    {
        Telecom = telecom;
    }


    public void Execute()
    {
        Telecom.Connect();
    }

    public void Undo()
    {
        Telecom.Disconnect();
    }
    
    public override string ToString()
    {
        return "ConnectTelecom";
    }
}