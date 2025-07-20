using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;

public partial class RestartLabel : Label
{
    EventBinding<StopGameEvent> stopGameEventBinding;
    EventBinding<RestartPointEvent> restartPointEventBinding;

    public override void _Ready()
    {
        stopGameEventBinding = new EventBinding<StopGameEvent>(OnStopGameEvent);
        restartPointEventBinding = new EventBinding<RestartPointEvent>(OnRestartPointEventBinding);
        EventBus<StopGameEvent>.Register(stopGameEventBinding);
        EventBus<RestartPointEvent>.Register(restartPointEventBinding);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Visible && Input.IsKeyPressed(Key.Space))
           EventBus<RestartPointEvent>.Raise(new RestartPointEvent());
    }

    private void OnStopGameEvent()
    {
        Visible = true;
    }

    private void OnRestartPointEventBinding()
    {
        Visible = false;
    }

    public override void _ExitTree()
    {
        EventBus<StopGameEvent>.Unregister(stopGameEventBinding);
        EventBus<RestartPointEvent>.Unregister(restartPointEventBinding);
        base._ExitTree();
    }
}
