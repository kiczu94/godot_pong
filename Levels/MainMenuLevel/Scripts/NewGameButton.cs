using Godot;

public partial class NewGameButton : Button
{
    public void OnButtonDown()
    {
        SceneManager.ChangeScene("res://Levels/GameLevel/GameScene.tscn");
    }
}
