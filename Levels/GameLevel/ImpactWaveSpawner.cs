using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;
using System.Collections.Generic;
using System.Linq;

public partial class ImpactWaveSpawner : Node
{
    EventBinding<BallHitWallEvent> ballHitWallEventBinding;
    EventBinding<ImpactWaveAnimationFinished> impactWaveAnimationFinsihedEventBinding;

    PackedScene impactWaveScene;

    HashSet<ImpactWave> notVisibleImpactWaves;
    HashSet<ImpactWave> visibleImpactWaves;

    int spawnedImpactWaves = 0;

    public override void _Ready()
    {
        impactWaveScene = ResourceLoader.Load("res://Entities/ImpactWave/ImpactWave.tscn") as PackedScene;
        ballHitWallEventBinding = new EventBinding<BallHitWallEvent>(OnBallHitWallEvent);
        impactWaveAnimationFinsihedEventBinding = new EventBinding<ImpactWaveAnimationFinished>(OnImpactWaveAnimationFinished);
        notVisibleImpactWaves = new HashSet<ImpactWave>();
        visibleImpactWaves = new HashSet<ImpactWave>();
        EventBus<BallHitWallEvent>.Register(ballHitWallEventBinding);
        EventBus<ImpactWaveAnimationFinished>.Register(impactWaveAnimationFinsihedEventBinding);
        base._Ready();
    }

    public override void _ExitTree()
    {
        EventBus<BallHitWallEvent>.Unregister(ballHitWallEventBinding);
        base._ExitTree();
    }

    private void OnBallHitWallEvent(BallHitWallEvent @event)
    {
        if (notVisibleImpactWaves.Count == 0)
        {
            SpawnNewImpactWave(@event.GlobalPosition);
            return;
        }
        SpawnFromPoolImpactWave(@event.GlobalPosition);
    }

    private void SpawnNewImpactWave(Vector2 spawnPosition)
    {
        var impactWaveNode = impactWaveScene.Instantiate() as ImpactWave;
        impactWaveNode.Name = $"ImpactWaveNode{spawnedImpactWaves}";
        AddChild(impactWaveNode);
        impactWaveNode.SetPosiition(spawnPosition);
        visibleImpactWaves.Add(impactWaveNode);
        spawnedImpactWaves++;
    }

    private void SpawnFromPoolImpactWave(Vector2 spawnPosition)
    {
        var impactWave = notVisibleImpactWaves.First();
        impactWave.SetPosiition(spawnPosition);
        impactWave.SetVisibility();
        notVisibleImpactWaves.Remove(impactWave);
        visibleImpactWaves.Add(impactWave);
    }

    private void OnImpactWaveAnimationFinished(ImpactWaveAnimationFinished @event)
    {
        var nodeToMove = GetChildren().Single(x => x.Name == @event.Name) as ImpactWave;
        visibleImpactWaves.Remove(nodeToMove);
        notVisibleImpactWaves.Add(nodeToMove);
    }
}
