using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;
using Pong_1.Scripts.Utilities;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 100f;

    public float EffectiveSpriteHeight { get; private set; }

    private bool isBallMoving;

    private CollisionShape2D playerCollisionShape;

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

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            SceneManager.ChangeScene("res://Levels/MainMenuLevel/MainMenu.tscn");
        }
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = GetPlayerVelocity();

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

    private void OnRestartPointEvent()
    {
        Position = new Vector2(40, 317);
    }

    public Vector2 GetPlayerVelocity()
    {
        if (Input.IsKeyPressed(Key.S))
        {
            return new Vector2(0, Speed);
        }
        if (Input.IsKeyPressed(Key.W))
        {
            return new Vector2(0, -Speed);
        }

        return Vector2.Zero;
    }

    public override void _ExitTree()
    {
        EventBus<BallHitLoseAreaEvent>.Unregister(ballHitLoseAreaEventBinding);
        EventBus<StartGameEvent>.Unregister(startGameEventBinding);
        EventBus<RestartPointEvent>.Unregister(restartPointEventBinding);
        base._ExitTree();
    }
}
