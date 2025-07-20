using Godot;
using Pong_1.Scripts.EventBus;
using Pong_1.Scripts.Events;

public partial class ImpactWave : Node
{
    Sprite2D _sprite;
    AnimationPlayer _player;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("ImpactWaveSprite");
        _player = GetNode<AnimationPlayer>("ImpactWaveAnimationPlayer");
        _player.AnimationFinished += OnAnimationFinished;
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (_sprite.Visible)
        {
            _player.Play("animation");
        }
        base._Process(delta);
    }

    public void SetPosiition(Vector2 position)
    {
        _sprite.Position = position;
    }

    public void SetVisibility()
    {
        _sprite.Visible = true;
    }

    public void OnAnimationFinished(StringName animationName)
    {
        _player.Stop();
        _sprite.Visible = false;
        EventBus<ImpactWaveAnimationFinished>.Raise(new ImpactWaveAnimationFinished(Name));
    }
}
