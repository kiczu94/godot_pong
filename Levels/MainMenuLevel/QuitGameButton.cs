using Godot;

public partial class QuitGameButton : Button
{
    public void OnButtonDown()
    {
        GetTree().Quit();
    }
}
