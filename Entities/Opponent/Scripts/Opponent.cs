using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;
using Pong_1.Scripts.Utilities;
using System.Threading.Tasks;

public partial class Opponent : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 150f;

    [Export]
    public Ball Ball { get; set; }

    public float EffectiveSpriteHeight { get; private set; }

    private Vector2 ballPosition;

    private bool toUpdateBallPosition = true;

    private bool isBallMoving = true;

    private EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;

    private EventBinding<StartGameEvent> startGameEventBinding;

    private EventBinding<RestartPointEvent> restartPointEventBinding;

    public override void _Ready()
    {
        EffectiveSpriteHeight =  GetNode<Sprite2D>("OpponentSprite").GetEffectiveTextureHeight();
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
        UpdateBallPosition();
        Velocity = GetUpdatedVelocity();
        MoveAndSlide();
        base._PhysicsProcess(delta);
    }

    private void UpdateBallPosition()
    {
        if (!toUpdateBallPosition) 
        {
            return;
        }

        ballPosition = Ball.Position;
        toUpdateBallPosition = false;
        _ = WaitForTime();
    }

    private Vector2 GetUpdatedVelocity()
    {
        if (ballPosition.Y < Position.Y)
        {
            return new Vector2(0, -Speed);
        }

        if (ballPosition.Y > Position.Y)
        {
            return new Vector2(0, Speed);
        }

        return Vector2.Zero;
    }

    private async Task WaitForTime()
    {
        await Task.Delay(1000);
        toUpdateBallPosition = true;
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
        Position = new Vector2(1107, 317);
    }

    public override void _ExitTree()
    {
        EventBus<BallHitLoseAreaEvent>.Unregister(ballHitLoseAreaEventBinding);
        EventBus<StartGameEvent>.Unregister(startGameEventBinding);
        EventBus<RestartPointEvent>.Unregister(restartPointEventBinding);
        base._ExitTree();
    }
}
