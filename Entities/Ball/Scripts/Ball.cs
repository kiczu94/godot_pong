using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;
using Pong_1.Scripts.Utilities;
using System.Linq;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 100;

    private int directionX = -1;

    private int directionY = 1;

    private bool isStopped = false;

    private float angle;

    private float _speed;


    private EventBinding<BallHitWallEvent> ballHitWallEventBinding;

    private EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;

    private EventBinding<BallHitPlayerEvent> ballHitPlayerEventBinding;

    private EventBinding<StartGameEvent> startGameEventBinding;

    private EventBinding<RestartPointEvent> restartPointEventBinding;

    public override void _Ready()
    {
        SetBindings();
        SetStartingMovementData();
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        var startBallPosition = new Vector2(Position.X, Position.Y);
        ProcessMovement();
        ProcessCollision(MoveAndCollide(Velocity * (float)delta), startBallPosition);
    }

    private void SetBindings()
    {
        ballHitWallEventBinding = new EventBinding<BallHitWallEvent>(OnBallHitWallEvent);
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseAreaEvent);
        restartPointEventBinding = new EventBinding<RestartPointEvent>(OnRestartPointEvent);
        ballHitPlayerEventBinding = new EventBinding<BallHitPlayerEvent>(OnBallHitPlayerEvent);
        startGameEventBinding = new EventBinding<StartGameEvent>(OnStartGameEvent);
        EventBus<BallHitPlayerEvent>.Register(ballHitPlayerEventBinding);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        EventBus<StartGameEvent>.Register(startGameEventBinding);
        EventBus<BallHitWallEvent>.Register(ballHitWallEventBinding);
        EventBus<RestartPointEvent>.Register(restartPointEventBinding);
    }

    private void SetStartingMovementData()
    {
        angle = Mathf.DegToRad(RandomGenerator<int>.PickRandom(5, 10, 15, 20, 25, 30, 35, 40, 45));
        directionX = RandomGenerator<int>.PickRandom(1, -1);
        directionY = RandomGenerator<int>.PickRandom(1, -1);
        _speed = Speed;
    }

    private void ProcessMovement()
    {
        if (_speed == 0)
        {
            Velocity = Vector2.Zero;
            return;
        }
        Velocity = new Vector2(_speed * directionX, _speed * directionY) * Vector2.FromAngle(angle);
    }

    private void ProcessCollision(KinematicCollision2D collision2D, Vector2 ballPosition)
    {
        if (collision2D == null)
        {
            return;
        }
        var colliderNode = this.GetColliderNode(collision2D);
        var colliderGroup = colliderNode.GetGroups().Single();

        if (colliderGroup == "Wall")
        {
            EventBus<BallHitWallEvent>.Raise(new BallHitWallEvent(Velocity, ballPosition));
        }

        if (colliderGroup == "Player")
        {
            var player = GetNode<CharacterBody2D>(this.GetColliderPath(collision2D)) as Player;
            EventBus<BallHitPlayerEvent>
                .Raise(new BallHitPlayerEvent(collision2D.GetPosition().Y, player.EffectiveSpriteHeight, player.GlobalPosition.Y));
        }

        if (colliderGroup == "Opponent")
        {
            var opponent = GetNode<CharacterBody2D>(this.GetColliderPath(collision2D)) as Opponent;
            EventBus<BallHitPlayerEvent>
                .Raise(new BallHitPlayerEvent(collision2D.GetPosition().Y, opponent.EffectiveSpriteHeight, opponent.GlobalPosition.Y));
        }

    }

    private void OnBallHitWallEvent(BallHitWallEvent @event) => directionY *= -1;

    private void OnBallHitLoseAreaEvent(BallHitLoseAreaEvent @event)
    {
        _speed = 0;
    }
    private void OnBallHitPlayerEvent(BallHitPlayerEvent @event)
    {
        Velocity = CalculateNewVelocity(@event);
    }

    private void OnStartGameEvent()
    {
        SetStartingMovementData();
    }

    private void OnRestartPointEvent()
    {
        SetPositionToCenter();
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

        return velocityWithHigherSpeed / Vector2.FromAngle(reflectionAngle);
    }

    private void UpdateMovementData(float reflectionAngle, int newYDirection)
    {
        directionX *= -1;
        angle = reflectionAngle;
        directionY = newYDirection;
    }

    public override void _ExitTree()
    {
        EventBus<BallHitPlayerEvent>.Unregister(ballHitPlayerEventBinding);
        EventBus<BallHitLoseAreaEvent>.Unregister(ballHitLoseAreaEventBinding);
        EventBus<StartGameEvent>.Unregister(startGameEventBinding);
        EventBus<BallHitWallEvent>.Unregister(ballHitWallEventBinding);
        EventBus<RestartPointEvent>.Unregister(restartPointEventBinding);
        base._ExitTree();
    }
}
