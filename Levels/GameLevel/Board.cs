using Godot;
using System.Linq;

public partial class Board : Parallax2D
{
    [Export]
    public bool ShowDividers { get; set; } = false;

    public override void _Ready()
    {
        if (ShowDividers)
        {
            var dividers = GetChildren().Where(x => x.IsInGroup("Divider")).Select(x => (Sprite2D)x);
            foreach (var divider in dividers)
            {
                divider.Visible = true;
            }
        }
        base._Ready();
    }
}
