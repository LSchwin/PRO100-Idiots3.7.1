using Godot;
using System;

public partial class KnifeSwing : Action
{
    Area2D Damage;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void Attack(int direc)
    {
        base.Attack(direc);
        SetPosition(direc, 16);
    }
}
