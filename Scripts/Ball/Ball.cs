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

    private int directionX = -1;

    private int directionY = 1;

    private bool isStopped = false;

    private float angle;

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
        angle = Mathf.DegToRad(45);
    }

    private void ProcessMovement()
    {
        if (isStopped) return;
        Velocity = new Vector2(Speed * directionX, Speed * directionY) * Vector2.FromAngle(angle);
    }

    private void ProcessCollision(KinematicCollision2D collision2D)
    {
        if (collision2D == null)
        {
            return;
        }
        var colliderNode = this.GetColliderNode(collision2D);
        var colliderGroup = colliderNode.GetGroups().Single();
        if (colliderGroup == "Wall")
            EventBus<BallHitWallEvent>.Raise(new BallHitWallEvent(Velocity));
        if (colliderGroup == "Player")
        {
            var player = GetNode<CharacterBody2D>(this.GetColliderPath(collision2D)) as Player;
            var dupa =player.playerCollisionShape;
        }
            
    }

    private void OnBallHitWallEvent(BallHitWallEvent @event) => directionY *= -1;

    private void OnBallHitLoseAreaEvent(BallHitLoseAreaEvent @event)
    {
        isStopped = true;
        Velocity = Vector2.Zero;
        SetPositionToCenter();
    }
    private void OnBallHitPlayerEvent(BallHitPlayerEvent @event)
    {
        directionX *= -1;
        Speed *= 1.05f;
        Velocity = CalculateNewVelocity();
    }

    private void SetPositionToCenter()
    {
        Position = new Vector2(576, 324);
    }

    private Vector2 CalculateNewVelocity()
    {
        var velocityWithHigherSpeed = new Vector2(Speed * directionX, Speed * directionY) * Vector2.FromAngle(this.angle);
        var speedWithChangedAngle = velocityWithHigherSpeed / Vector2.FromAngle(Mathf.DegToRad(30));
        return speedWithChangedAngle * Vector2.FromAngle(Mathf.DegToRad(30));

    }
}
