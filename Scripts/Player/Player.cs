using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed { get; set; } = 150;

    private CollisionShape2D playerCollisionShape;

    public override void _Ready()
    {
        playerCollisionShape = GetNode<CollisionShape2D>("PlayerCollisionShape");
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
        base._PhysicsProcess(delta);
        MoveAndSlide();
    }
}
