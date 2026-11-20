using Godot;
using System;

public partial class KnifeSwing : Action
{
    Area2D Damage;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void Attack(Direction direction)
    {
        base.Attack(direction);
        SetPosition(direction, 16);
    }
}
