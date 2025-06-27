using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;
using Pong_1.Scripts.Events;
using Pong_1.Scripts.Utilities;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 150;

    public float EffectiveSpriteHeight { get; set; }

    private CollisionShape2D playerCollisionShape;

    private bool isBallMoving;

    private EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;

    private EventBinding<StartGameEvent> startGameEventBinding;

    private EventBinding<RestartPointEvent> restartPointEventBinding;

    public override void _Ready()
    {
        playerCollisionShape = GetNode<CollisionShape2D>("PlayerCollisionShape");
        EffectiveSpriteHeight = GetNode<Sprite2D>("PlayerSprite").GetEffectiveTextureHeight();
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseAreaEventBiding);
        startGameEventBinding = new EventBinding<StartGameEvent>(OnStartGameEventBinding);
        restartPointEventBinding = new EventBinding<RestartPointEvent>(OnRestartPointEvent);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        EventBus<StartGameEvent>.Register(startGameEventBinding);
        EventBus<RestartPointEvent>.Register(restartPointEventBinding);
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = Vector2.Zero;
        if (Input.IsKeyPressed(Key.W))
        {
            Velocity = new Vector2(0, -Speed);
        }

        if (Input.IsKeyPressed(Key.S))
        {
            Velocity = new Vector2(0, Speed);
        }

        MoveAndSlide();

        base._PhysicsProcess(delta);
    }

    private void OnBallHitLoseAreaEventBiding()
    {
        isBallMoving = false;
        Speed = 0;
    }

    private void OnStartGameEventBinding()
    {
        isBallMoving = true;
        Speed = 150;
    }

    private void OnRestartPointEvent(RestartPointEvent @event)
    {
        Position = new Vector2(40, 317);
    }
}
