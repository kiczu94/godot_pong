using pong_1.Scripts.EventBus;

namespace pong_1.Scripts.Events;

public record BallHitLoseAreaEvent(bool leftArea) : IEvent;
