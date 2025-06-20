using System.Linq;
using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;
using pong_1.Scripts.Utilities;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 500f;

    private int directionX;

    private int directionY;

    private bool isStopped = false;

    private EventBinding<BallHitWallEvent> ballHitWallEventBinding;

    private EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;


    public override void _Ready()
    {
        ballHitWallEventBinding = new EventBinding<BallHitWallEvent>(OnBallHitWallEvent);
        EventBus<BallHitWallEvent>.Register(ballHitWallEventBinding);
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseAreaEvent);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        directionX = RandomGenerator<int>.PickRandom(-1, 1);
        directionY = RandomGenerator<int>.PickRandom(-1, 1);
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessMovement();
        ProcessCollision(MoveAndCollide(Velocity * (float)delta));
    }

    private void ProcessMovement()
    {
        if (isStopped) return;
        Velocity = new Vector2(Speed * directionX, Speed * directionY);
    }

    private void ProcessCollision(KinematicCollision2D collision2D)
    {
        if (collision2D == null)
        {
            return;
        }
        var colliderGroups = this.GetColliderGroups(collision2D);
        if (colliderGroups.ToList().Contains("Wall"))
            EventBus<BallHitWallEvent>.Raise(new BallHitWallEvent(Velocity));
    }

    private void OnBallHitWallEvent(BallHitWallEvent @event) => directionY *= -1;

    private void OnBallHitLoseAreaEvent(BallHitLoseAreaEvent @event)
    {
        isStopped = true;
        Velocity = Vector2.Zero;
        SetPositionToCenter();
    }
    private void SetPositionToCenter()
    {
        Position = new Vector2(576, 324);
    }
}
