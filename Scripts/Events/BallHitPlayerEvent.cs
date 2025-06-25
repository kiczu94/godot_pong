using pong_1.Scripts.EventBus;

namespace Pong_1.Scripts.Events;

public record BallHitPlayerEvent(float collisionY, int playerHeight, float playerPosition) : IEvent;
