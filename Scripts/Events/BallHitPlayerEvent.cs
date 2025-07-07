using Pong_1.Scripts.EventBus;

namespace Pong_1.Scripts.Events;

public record BallHitPlayerEvent(float collisionY, float playerHeight, float playerPosition) : IEvent;
