using Godot;
using Pong_1.Scripts.EventBus;

namespace Pong_1.Scripts.Events;

public record BallHitWallEvent(Vector2 velocity) : IEvent;
