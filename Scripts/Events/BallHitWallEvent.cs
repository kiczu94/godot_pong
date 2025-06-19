
using Godot;
using pong_1.Scripts.EventBus;

namespace pong_1.Scripts.Events;

    public record BallHitWallEvent(Vector2 velocity) : IEvent;
