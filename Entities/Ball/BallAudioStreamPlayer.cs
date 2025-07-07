using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;

public partial class BallAudioStreamPlayer : AudioStreamPlayer
{
    AudioStreamWav hitWallOrPlayerSound;
    AudioStreamWav hitLoseAreaSound;
    EventBinding<BallHitPlayerEvent> ballHitPlayerEventBinding;
    EventBinding<BallHitWallEvent> ballHitWallEventBinding;
    EventBinding<BallHitLoseAreaEvent> ballHitLoseAreaEventBinding;

    public override void _Ready()
    {
        ballHitPlayerEventBinding = new EventBinding<BallHitPlayerEvent>(OnBallHitPlayer);
        ballHitWallEventBinding = new EventBinding<BallHitWallEvent>(OnBallHitWall);
        ballHitLoseAreaEventBinding = new EventBinding<BallHitLoseAreaEvent>(OnBallHitLoseArena);
        EventBus<BallHitPlayerEvent>.Register(ballHitPlayerEventBinding);
        EventBus<BallHitWallEvent>.Register(ballHitWallEventBinding);
        EventBus<BallHitLoseAreaEvent>.Register(ballHitLoseAreaEventBinding);
        hitWallOrPlayerSound = ResourceLoader.Load("res://Entities/Ball/Sounds/768851__etheraudio__pulse-loud-pitch-bend.wav") as AudioStreamWav;
        hitLoseAreaSound = ResourceLoader.Load("res://Entities/Ball/Sounds/815346__etheraudio__crusty-retro-jingle.wav") as AudioStreamWav;
        base._Ready();
    }

    public override void _ExitTree()
    {
        EventBus<BallHitPlayerEvent>.Unregister(ballHitPlayerEventBinding);
        EventBus<BallHitWallEvent>.Unregister(ballHitWallEventBinding);
        EventBus<BallHitLoseAreaEvent>.Unregister(ballHitLoseAreaEventBinding);
        base._ExitTree();
    }

    private void OnBallHitPlayer()
    {
        PlaySound(hitWallOrPlayerSound);
    }

    private void OnBallHitWall()
    {
        PlaySound(hitWallOrPlayerSound);
    }

    private void OnBallHitLoseArena()
    {
        PlaySound(hitLoseAreaSound);
    }

    private void PlaySound(AudioStreamWav sound)
    {
        if (!Playing)
        {
            Stream = sound;
            Play();
        }
    }
}
