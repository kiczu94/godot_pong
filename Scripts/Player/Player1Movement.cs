using Godot;

namespace Pong_1.Scripts.Player;

[GlobalClass]
public partial class Player1Movement : PlayerMovement
{
    [Export]
    public new float Speed { get; set; }

    public override Vector2 GetVelocity()
    {
        if (Input.IsKeyPressed(Key.S))
        {
            return new Vector2(0, Speed);
        }
        if (Input.IsKeyPressed(Key.W))
        {
            return new Vector2(0, -Speed);
        }

        return Vector2.Zero;
    }
}
