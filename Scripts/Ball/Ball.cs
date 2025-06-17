using Godot;
using pong_1.Scripts.Utilities;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 30f;

    int directionX;
    int directionY;

    CollisionShape2D ballCollisionShape;


    public override void _Ready()
    {
        base._Ready();
        ballCollisionShape = GetNode<CollisionShape2D>("BallCollisionShape");
        directionX = RandomGenerator<int>.PickRandom(-1, 1);
        directionY = RandomGenerator<int>.PickRandom(-1, 1);
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = new Vector2(Speed * directionX, Speed * directionY);
        MoveAndSlide();
    }


}
