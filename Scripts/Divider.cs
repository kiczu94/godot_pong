using Godot;

public partial class Divider : Sprite2D
{
    public override void _Ready()
    {
        GD.Print($"Divider height {Texture.GetHeight()}");
        GD.Print($"Divider Width {Texture.GetWidth()}");
        base._Ready();
    }
}
