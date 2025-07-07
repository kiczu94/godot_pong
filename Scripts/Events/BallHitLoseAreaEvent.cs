using Pong_1.Scripts.EventBus;

namespace Pong_1.Scripts.Events;

public record BallHitLoseAreaEvent(bool leftArea) : IEvent;
