using System;

namespace pong_1.Scripts.EventBus;

public interface IEventBinding<T>
{
    public Action<T> onEvent { get; set; }

    public Action onEventNoArgs { get; set; }

    public void Add(Action<T> action);

    public void Remove(Action<T> action);

    public void Add(Action action);

    public void Remove(Action action);
}

public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
    public Action<T> onEvent = _ => { };

    public Action onEventNoArgs = () => { };

    Action<T> IEventBinding<T>.onEvent { get => onEvent; set => onEvent = value; }

    Action IEventBinding<T>.onEventNoArgs { get => onEventNoArgs; set => onEventNoArgs = value; }

    public EventBinding(Action<T> onEvent) { this.onEvent = onEvent; }

    public EventBinding(Action onEventNoArgs) { this.onEventNoArgs = onEventNoArgs; }

    public void Add(Action<T> action) => this.onEvent += action;

    public void Add(Action action) => this.onEventNoArgs += action;

    public void Remove(Action<T> action) => this.onEvent -= action;

    public void Remove(Action action) => this.onEventNoArgs -= action;

}
