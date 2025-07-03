using Godot;

namespace Pong_1.Scripts.Player;

[GlobalClass]
public partial class PlayerMovement : Resource
{
    [Export]
    public float Speed { get; set; } = 150f;

    public virtual Vector2 GetVelocity()
    {
        if (Input.IsKeyPressed(Key.W))
        {
            return new Vector2(0, -Speed);
        }

        if (Input.IsKeyPressed(Key.S))
        {
            return new Vector2(0, Speed);
        }

        return Vector2.Zero;
    }
}
