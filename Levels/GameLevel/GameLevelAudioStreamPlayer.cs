using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;
using Pong_1.Scripts.Utilities;

public partial class GameLevelAudioStreamPlayer : AudioStreamPlayer
{
    AudioStreamWav hitWallOrPlayerSound;
    AudioStreamWav hitLoseAreaSound;
    AudioStreamWav player1WonSound;
    AudioStreamWav player2WonSound;
    EventBinding<BallHitPlayerEvent> ballHitPlayerEventBinding;
    EventBinding<BallHitWallEvent> ballHitWallEventBinding;
    EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;
    EventBinding<GameFinishedEvent> gameFinishedEventBinding;

    public override void _Ready()
    {
        ballHitPlayerEventBinding = new EventBinding<BallHitPlayerEvent>(OnBallHitPlayer);
        ballHitWallEventBinding = new EventBinding<BallHitWallEvent>(OnBallHitWall);
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseArena);
        gameFinishedEventBinding = new EventBinding<GameFinishedEvent>(OnGameFinished);
        EventBus<BallHitPlayerEvent>.Register(ballHitPlayerEventBinding);
        EventBus<BallHitWallEvent>.Register(ballHitWallEventBinding);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        EventBus<GameFinishedEvent>.Register(gameFinishedEventBinding);
        hitWallOrPlayerSound = ResourceLoader.Load("res://Entities/Ball/Sounds/768851__etheraudio__pulse-loud-pitch-bend.wav") as AudioStreamWav;
        hitLoseAreaSound = ResourceLoader.Load("res://Entities/Ball/Sounds/815346__etheraudio__crusty-retro-jingle.wav") as AudioStreamWav;
        player1WonSound = ResourceLoader.Load("res://UI/Sounds/810330__mokasza__triumphant-success.mp3") as AudioStreamWav;
        player2WonSound = ResourceLoader.Load("res://UI/Sounds/171673__leszek_szary__failure-1.wav") as AudioStreamWav;
        base._Ready();
    }

    public override void _ExitTree()
    {
        EventBus<BallHitPlayerEvent>.Unregister(ballHitPlayerEventBinding);
        EventBus<BallHitWallEvent>.Unregister(ballHitWallEventBinding);
        EventBus<BallHitLoseAreaEvent>.Unregister(ballHitLoseAreaEventBinding);
        EventBus<GameFinishedEvent>.Unregister(gameFinishedEventBinding);
        base._ExitTree();
    }

    private void OnBallHitPlayer()
    {
        this.PlaySound(hitWallOrPlayerSound);
    }

    private void OnBallHitWall()
    {
        this.PlaySound(hitWallOrPlayerSound);
    }

    private void OnBallHitLoseArena()
    {
        this.PlaySound(hitLoseAreaSound);
    }
    private void OnGameFinished(GameFinishedEvent @event)
    {
        if (@event.player1Won)
        {
            this.PlaySound(player1WonSound);
        }
        else
        {
            this.PlaySound(player2WonSound);
        }
    }
}
