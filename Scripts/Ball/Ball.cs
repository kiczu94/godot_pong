using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;
using pong_1.Scripts.Utilities;
using Pong_1.Scripts.Events;
using System.Linq;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 10f;

    private int directionX;

    private int directionY;

    private bool isStopped = false;

    private float angleX;

    private float angleY;

    private EventBinding<BallHitWallEvent> ballHitWallEventBinding;

    private EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;

    private EventBinding<BallHitPlayerEvent> ballHitPlayerEventBinding;


    public override void _Ready()
    {
        SetBindings();
        SetBallMovementData();
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessMovement();
        ProcessCollision(MoveAndCollide(Velocity * (float)delta));
    }

    private void SetBindings()
    {
        ballHitWallEventBinding = new EventBinding<BallHitWallEvent>(OnBallHitWallEvent);
        EventBus<BallHitWallEvent>.Register(ballHitWallEventBinding);
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseAreaEvent);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        ballHitPlayerEventBinding = new EventBinding<BallHitPlayerEvent>(OnBallHitPlayerEvent);
        EventBus<BallHitPlayerEvent>.Register(ballHitPlayerEventBinding);
    }

    private void SetBallMovementData()
    {
        directionX = RandomGenerator<int>.PickRandom(-1, 1);
        directionY = RandomGenerator<int>.PickRandom(-1, 1);
        angleX = Mathf.DegToRad(45);
        angleY = Mathf.DegToRad(45);
    }

    private void ProcessMovement()
    {
        if (isStopped) return;
        Velocity = new Vector2(Speed * directionX * angleX, Speed * directionY * angleY);
    }

    private void ProcessCollision(KinematicCollision2D collision2D)
    {
        if (collision2D == null)
        {
            return;
        }
        var colliderGroups = this.GetColliderGroups(collision2D).ToList();
        if (colliderGroups.Contains("Wall"))
            EventBus<BallHitWallEvent>.Raise(new BallHitWallEvent(Velocity));
        if (colliderGroups.Contains("Player"))
            EventBus<BallHitPlayerEvent>.Raise(new BallHitPlayerEvent());
    }

    private void OnBallHitWallEvent(BallHitWallEvent @event) => directionY *= -1;

    private void OnBallHitLoseAreaEvent(BallHitLoseAreaEvent @event)
    {
        isStopped = true;
        Velocity = Vector2.Zero;
        SetPositionToCenter();
    }
    private void OnBallHitPlayerEvent(BallHitPlayerEvent @event) => directionX *= -1;

    private void SetPositionToCenter()
    {
        Position = new Vector2(576, 324);
    }
}
