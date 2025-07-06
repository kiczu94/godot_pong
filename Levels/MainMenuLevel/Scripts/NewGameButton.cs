using Godot;
using pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;

public partial class NewGameButton : Button
{
    public void OnButtonDown()
    {
        EventBus<StartNewGameEvent>.Raise(new StartNewGameEvent());
    }
}
