using Godot;
using System;

public partial class Ghost : Sprite2D
{
    [Export]
    float Lifespan = 1.0f;
    public override void _Ready()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate:a", 0.0, Lifespan).SetTrans(Tween.TransitionType.Quart);
        tween.Finished += () => Despawn();
    }

    private void Despawn()
    {
        QueueFree();
    }
}
