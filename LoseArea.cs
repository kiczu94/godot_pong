using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;

public partial class LoseArea : Area2D
{
    [Export]
    public bool IsLeft { get; set; }

    public override void _Ready()
    {
        base._Ready();
    }

    public void OnBodyEntered(Node2D body)
    {
        EventBus<BallHitLoseAreaEvent>.Raise(new BallHitLoseAreaEvent(IsLeft));
    }

}
