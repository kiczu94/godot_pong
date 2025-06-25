using Godot;
using pong_1.Scripts.EventBus;
using pong_1.Scripts.Events;
using pong_1.Scripts.Utilities;
using Pong_1.Scripts.Events;
using System.Linq;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 150;

    public CollisionShape2D playerCollisionShape;
    private Sprite2D playerSprite;

    public override void _Ready()
    {
        playerCollisionShape = GetNode<CollisionShape2D>("PlayerCollisionShape");
        playerSprite = GetNode<Sprite2D>("PlayerSprite");
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = Vector2.Zero;
        if (Input.IsKeyPressed(Key.W))
        {
            Velocity = new Vector2(0, -Speed);
        }

        if (Input.IsKeyPressed(Key.S))
        {
            Velocity = new Vector2(0, Speed);
        }

        var collision = MoveAndCollide(Velocity * (float)delta);

        ProcessCollision(collision);

        base._PhysicsProcess(delta);
    }

    private void ProcessCollision(KinematicCollision2D collision2D)
    {
        if (collision2D == null)
        {
            return;
        }
        var colliderGroups = this.GetColliderGroups(collision2D);
        if (colliderGroups.Contains("Ball"))
            EventBus<BallHitPlayerEvent>.Raise(new BallHitPlayerEvent(collision2D.GetPosition().Y, playerSprite.Texture.GetHeight(), this.Position.Y));
    }
}
