using Godot;
using System;

public partial class Action : Node2D
{
    [Export]
    public Entity entityOwner = null;
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
    [Export]
    public float ChargeUpTime { get; set
        {
            field = (value < 0.0f) ? 0.0f: value;
        }
    }
    protected Timer lifespan = null;
    protected Timer chargeUp = null;
    protected bool chargingUp = false;
    public ETeam Team { get; set; }
    // Team is set by the owner of the action
    // Remember to do that or else the action will damage the owner

    public override void _Ready()
    {
        
        if(Lifespan > 0.0f)
        {
            lifespan = GetNode<Timer>("Lifespan");
            lifespan.WaitTime = Lifespan;
            lifespan.Timeout += () => Despawn();
        }
        if (ChargeUpTime > 0.0f)
        {
            chargeUp = GetNode<Timer>("ChargeUp");
            chargeUp.WaitTime = ChargeUpTime;
            chargeUp.Timeout += () => ChargeUpFinish();
        }
        // If Lifespan is less than 0, the action will never despawn
        else if (Lifespan > 0.0f)
        {
            lifespan.Start();
        }
    }

    public Entity GetActionOwner() { return entityOwner; }
    public void SetActionOwner(Entity newOwner) { entityOwner = newOwner; }

    public virtual void Attack(Direction direc) 
    {
        if (ChargeUpTime > 0.0f)
        {
            chargingUp = true;
            chargeUp.Start();
        }
        DespawnAction += () => AttackFinish();
    }

    public void AttackFinish()
    {
        DespawnAction -= () => AttackFinish();
    }

    [Signal]
    public delegate void DespawnActionEventHandler();

    public virtual void Despawn()
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

    protected virtual void ChargeUpFinish()
    {
        lifespan.Start();
        chargingUp = false;
    }
}
