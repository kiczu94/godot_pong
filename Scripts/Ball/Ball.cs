using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;
using pong_1.Scripts.Utilities;
using Pong_1.Scripts.Events;
using System.Linq;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float Speed { get; set; }

    private int directionX = -1;

    private int directionY = 1;

    private bool isStopped = false;

    private float angle;

    private float _speed;

    private EventBinding<BallHitWallEvent> ballHitWallEventBinding;

    private EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;

    private EventBinding<BallHitPlayerEvent> ballHitPlayerEventBinding;

    private EventBinding<StartGameEvent> startGameEventBinding;


    public override void _Ready()
    {
        SetBindings();
        SetStartingMovementData();
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
        startGameEventBinding = new EventBinding<StartGameEvent>(OnStartGameEvent);
        EventBus<StartGameEvent>.Register(startGameEventBinding);
    }

    private void SetStartingMovementData()
    {
        angle = Mathf.DegToRad(RandomGenerator<int>.PickRandom(5,10,15,20,25,30,35,40,45));
        directionX = RandomGenerator<int>.PickRandom(1, -1);
        directionY = RandomGenerator<int>.PickRandom(1, -1);
        _speed = Speed;
    }

    private void ProcessMovement()
    {
        if (_speed == 0) return;
        Velocity = new Vector2(_speed * directionX, _speed * directionY) * Vector2.FromAngle(angle);
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
        {
            EventBus<BallHitWallEvent>.Raise(new BallHitWallEvent(Velocity));
        }

        if (colliderGroup == "Player")
        {
            var player = GetNode<CharacterBody2D>(this.GetColliderPath(collision2D)) as Player;
            EventBus<BallHitPlayerEvent>
                .Raise(new BallHitPlayerEvent(collision2D.GetPosition().Y, player.EffectiveSpriteHeight, player.GlobalPosition.Y));
        }

    }

    private void OnBallHitWallEvent(BallHitWallEvent @event) => directionY *= -1;

    private void OnBallHitLoseAreaEvent(BallHitLoseAreaEvent @event)
    {
        _speed = 0;
        SetPositionToCenter();
    }
    private void OnBallHitPlayerEvent(BallHitPlayerEvent @event)
    {
        Velocity = CalculateNewVelocity(@event);
    }

    private void OnStartGameEvent()
    {
        SetStartingMovementData();
    }

    private void SetPositionToCenter()
    {
        Position = new Vector2(576, 324);
    }

    private Vector2 CalculateNewVelocity(BallHitPlayerEvent @event)
    {
        var hitPlayerSegment = GetSegmentNumber(@event);
        var reflectionAngle = GetReflectionAngle(hitPlayerSegment);
        var newYDirection = GetReflectedYDirection(hitPlayerSegment);
        var newSpeed = GetNewSpeed(reflectionAngle);

        UpdateMovementData(reflectionAngle, newYDirection);
        
        return new Vector2(newSpeed.X * directionX, newSpeed.Y * directionY) * Vector2.FromAngle(angle);
    }

    private int GetSegmentNumber(BallHitPlayerEvent @event)
    {
        var playerSegmentHeight = @event.playerHeight / 9; //just to easy divide 45 degrees
        var diff = @event.playerPosition - @event.collisionY;
        
        return (int)Mathf.Round(diff / playerSegmentHeight);
    }

    private float GetReflectionAngle(int playerSegmentNumber) => playerSegmentNumber switch
    {
        0 => Mathf.DegToRad(5),
        1 or -1 => Mathf.DegToRad(15),
        2 or -2 => Mathf.DegToRad(25),
        3 or -3 => Mathf.DegToRad(35),
        4 or -4 => Mathf.DegToRad(45),
        _ => Mathf.DegToRad(5),
    };

    private int GetReflectedYDirection(int playerSegmentNumber) => playerSegmentNumber switch
    {
        >= 0 => -1,
        _ => 1
    };

    private Vector2 GetNewSpeed(float reflectionAngle)
    {
        _speed *= 1.1f;
        var velocityWithHigherSpeed = new Vector2(_speed * directionX, _speed * directionY) * Vector2.FromAngle(this.angle);
        
        return  velocityWithHigherSpeed / Vector2.FromAngle(reflectionAngle);
    }

    private void UpdateMovementData(float reflectionAngle, int newYDirection)
    {
        directionX *= -1;
        angle = reflectionAngle;
        directionY = newYDirection;
    }
}
