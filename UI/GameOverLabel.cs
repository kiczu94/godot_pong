using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;

public partial class GameOverLabel : Label
{
    EventBinding<GameFinishedEvent> gameFinishedEventBinding;

    public override void _Ready()
    {
        Visible = false;
        gameFinishedEventBinding = new EventBinding<GameFinishedEvent>(OnGameFinishedEvent);
        EventBus<GameFinishedEvent>.Register(gameFinishedEventBinding);
        base._Ready();
    }

    public void OnGameFinishedEvent(GameFinishedEvent @event)
    {
        if(@event.player1Won)
        {
            Text = $"GAME OVER\nPLAYER 1 WON\nPRESS ESC TO RETURN TO MAIN MENU";
        }
        else
        {
            Text = $"GAME OVER\nPLAYER 2 WON\nPRESS ESC TO RETURN TO MAIN MENU";
        }
        
        Visible = true;
    }

    public override void _ExitTree()
    {
        EventBus<GameFinishedEvent>.Unregister(gameFinishedEventBinding);
        base._ExitTree();
    }
}
