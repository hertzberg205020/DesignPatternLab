using ShortCutSettings.Controllers;

namespace ShortCutSettings.Commands;

public class ResetMainControlKeyboardCommand: ICommand
{
    private MainController MainController {get; init;}
    
    public ResetMainControlKeyboardCommand(MainController mainController)
    {
        MainController = mainController;
    }
    
    public void Execute()
    {
        MainController.ResetKeyboardSetting();
    }

    public void Undo()
    {
        MainController.RestoreKeyboardSetting();
    }
    
    public override string ToString()
    {
        return "ResetMainControlKeyboard";
    }
}