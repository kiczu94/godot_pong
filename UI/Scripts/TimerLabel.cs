using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;
using System.Threading.Tasks;

public partial class TimerLabel : Label
{
    private EventBinding<RestartPointEvent> restartPointEventBinding;

    private EventBinding<ChangeVisibleTimerValueEvent> changeVisibleTimerValueEventBinding;

    private int timeLeft = 3;

    public override void _Ready()
    {
        Visible = false;
        restartPointEventBinding = new EventBinding<RestartPointEvent>(OnRestartPointEvent);
        changeVisibleTimerValueEventBinding = new EventBinding<ChangeVisibleTimerValueEvent>(OnChangeVisibleTimerValueEvent);
        EventBus<RestartPointEvent>.Register(restartPointEventBinding);
        EventBus<ChangeVisibleTimerValueEvent>.Register(changeVisibleTimerValueEventBinding);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        Text = timeLeft.ToString();
        base._Process(delta);
    }

    private async Task OnRestartPointEvent()
    {
        Visible = true;
        await Task.Delay(1000);
        EventBus<ChangeVisibleTimerValueEvent>.Raise(new ChangeVisibleTimerValueEvent());
    }

    private async Task OnChangeVisibleTimerValueEvent()
    {
        timeLeft -= 1;
        await Task.Delay(1000);
        if (timeLeft <= 0)
        {
            Visible = false;
            timeLeft = 3;
            EventBus<StartGameEvent>.Raise(new StartGameEvent());
        }
        else
        {
            EventBus<ChangeVisibleTimerValueEvent>.Raise(new ChangeVisibleTimerValueEvent());
        }
    }

    public override void _ExitTree()
    {
        EventBus<RestartPointEvent>.Unregister(restartPointEventBinding);
        EventBus<ChangeVisibleTimerValueEvent>.Unregister(changeVisibleTimerValueEventBinding);
        base._ExitTree();
    }
}
