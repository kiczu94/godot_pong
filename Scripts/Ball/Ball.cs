using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;
using pong_1.Scripts.Utilities;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 500f;

    int directionX;
    int directionY;

    EventBinding<BallHitWallEvent> ballHitWallEventBinding;

    public override void _Ready()
    {
        base._Ready();
        ballHitWallEventBinding = new EventBinding<BallHitWallEvent>(OnBallHitWallEvent);
        EventBus<BallHitWallEvent>.Register(ballHitWallEventBinding);
        directionX = RandomGenerator<int>.PickRandom(-1, 1);
        directionY = RandomGenerator<int>.PickRandom(-1, 1);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = new Vector2(Speed * directionX, Speed * directionY);
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null)
        {
            var colliderGroups = collision.GetColliderGroup();
            var colliderId = collision.GetColliderId();
            var instance = InstanceFromId(colliderId);
            
            EventBus<BallHitWallEvent>.Raise(new BallHitWallEvent(Velocity));
        }
    }

    private void OnBallHitWallEvent(BallHitWallEvent @event)
    {
        directionY *= -1;
    }

}
