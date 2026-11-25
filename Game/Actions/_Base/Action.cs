using Godot;
using System;

public partial class Action : Node2D
{
    [Export]
    private Entity entityOwner = null;
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
    public ETeam Team { get; set; }

    public override void _Ready()
    {
        lifespan = GetNode<Timer>("Lifespan");
        lifespan.WaitTime = Lifespan;
        lifespan.Timeout += () => Despawn();
        lifespan.Start();
    }

    public Entity GetActionOwner() { return entityOwner; }
    public void SetActionOwner(Entity newOwner) { entityOwner = newOwner; }

    public virtual void Attack(Direction direc) 
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

    public enum Direction { Left = 1, Up = 2, Right = 3 , Down = 4 }
    public void SetPosition(Direction direction = Direction.Left, int offset = 0)
    {
        Vector2 SwingPos = Position;
        switch (direction)
        {
            case Direction.Left:
                Rotation = float.Pi;
                SwingPos.X -= 16;
                break;
            case Direction.Up:
                Rotation = (3 * float.Pi) / 2;
                SwingPos.Y -= 16;
                break;
            case Direction.Down:
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
