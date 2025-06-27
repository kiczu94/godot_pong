using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;
using Pong_1.Scripts.Events;

public partial class RestartLabel : Label
{
    EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;
    EventBinding<RestartPointEvent> restartPointEventBinding;

    public override void _Ready()
    {
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseAreEvent);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        restartPointEventBinding = new EventBinding<RestartPointEvent>(OnRestartPointEventBinding);
        EventBus<RestartPointEvent>.Register(restartPointEventBinding);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Visible && Input.IsKeyPressed(Key.Space))
            EventBus<RestartPointEvent>.Raise(new RestartPointEvent());
    }

    private void OnBallHitLoseAreEvent(BallHitLoseAreaEvent @event)
    {
        Visible = true;
    }

    private void OnRestartPointEventBinding()
    {
        Visible = false;
    }
}
