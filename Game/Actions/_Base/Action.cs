using Godot;
using System;

public partial class Action : Node2D
{
    [Export]
    private Entity owner = null;
    [Export]
    public float Lifespan
    {
        get;
        set
        {
            if (value < 0.0f) field = 0.0f;
            else field = value;
        }
    }
    private Timer lifespan = null;

    public override void _Ready()
    {
        lifespan = GetNode<Timer>("Lifespan");
        lifespan.WaitTime = Lifespan;
        lifespan.Timeout += () => Despawn();
        lifespan.Start();
    }

    public Entity GetActionOwner() { return owner; }
    public void SetActionOwner(Entity newOwner) { owner = newOwner; }

    public virtual void Attack(int direc) 
    {
        DespawnAction += () => AttackFinish();
    }

    public void AttackFinish()
    {
        DespawnAction -= () => AttackFinish();
    }

    [Signal]
    public delegate void DespawnActionEventHandler();

    public void Despawn()
    {
        EmitSignal(SignalName.DespawnAction);
        QueueFree();
        
    }

    public void SetPosition(int direc = 1, int offset = 0)
    {
        Vector2 SwingPos = Position;
        switch (direc)
        {
            // 1 is left, 2 is up, 3 is right, 4 is down
            case 1:
                Rotation = float.Pi;
                SwingPos.X -= 16;
                break;
            case 2:
                Rotation = (3 * float.Pi) / 2;
                SwingPos.Y -= 16;
                break;
            case 4:
                Rotation = float.Pi / 2;
                SwingPos.Y += 16;
                break;
            default:
                Rotation = 0;
                SwingPos.X += 16;
                break;
        }
        Position = SwingPos;
    }
}
