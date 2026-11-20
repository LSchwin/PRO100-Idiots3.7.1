using Godot;
using System;

public partial class Projectile : CharacterBody2D
{
    [Export]
    int MoveSpeed;
    public int Direc;
    private DamageArea DamageArea;

    public override void _Ready()
    {
        base._Ready();
        DamageArea = GetNode<DamageArea>("DamageArea");
        DamageArea.Hit += Collide;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 NewVel = Velocity;
        switch (Direc)
        {
            case 1:
                NewVel.X = -MoveSpeed;
                break;
            case 2:
                NewVel.Y = -MoveSpeed;
                break;
            case 3:
                NewVel.X = MoveSpeed;
                break;
            default:
                NewVel.Y = MoveSpeed;
                break;
        }

        Velocity = NewVel;
        MoveAndSlide();
    }

    private void Collide()
    {
        DamageArea.Hit -= Collide;
        QueueFree();
    }
}
