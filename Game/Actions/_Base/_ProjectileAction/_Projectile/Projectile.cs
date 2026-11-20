using Godot;
using System;

public partial class Projectile : CharacterBody2D
{
    [Export]
    int MoveSpeed;
    [Export]
    bool CanDespawn = true;
    [Export]
    float Duration = 1.0f;
    public int Direc;
    private Timer Lifespan;
    private DamageArea DamageArea;

    public override void _Ready()
    {
        base._Ready();
        Lifespan = GetNode<Timer>("Lifespan");
        DamageArea = GetNode<DamageArea>("DamageArea");
        DamageArea.Hit += Damage;
        DamageArea.Collide += Collide;
        if (CanDespawn)
        {
            Lifespan.Timeout += () => QueueFree();
            Lifespan.Start();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 NewVel = Velocity;
        switch (Direc)
        {
            case 1:
                Rotation = float.Pi;
                NewVel.X = -MoveSpeed;
                break;
            case 2:
                Rotation = (3* float.Pi)/2;
                NewVel.Y = -MoveSpeed;
                break;
            case 3:
                NewVel.X = MoveSpeed;
                break;
            default:
                Rotation = float.Pi / 2;
                NewVel.Y = MoveSpeed;
                break;
        }

        Velocity = NewVel;
        MoveAndSlide();
    }

    private void Damage()
    {
        DamageArea.Hit -= Damage;
        QueueFree();
    }

    private void Collide()
    {
        DamageArea.Collide -= Collide;
        QueueFree();
    }
}
