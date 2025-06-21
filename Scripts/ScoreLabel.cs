using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;

public partial class ScoreLabel : Label
{
    public int Player1Score { get; set; } = 0;

    public int Player2Score { get; set; } = 0;

    EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;

    public override void _Ready()
    {
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseAreEvent);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        Text = $"{Player1Score} : {Player2Score}";
        base._Process(delta);
    }


    private void OnBallHitLoseAreEvent(BallHitLoseAreaEvent @event)
    {
        if (@event.leftArea) Player2Score += 1;
        else Player1Score += 1;
    }
}
