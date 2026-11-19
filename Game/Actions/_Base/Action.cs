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

    

    [Signal]
    public delegate void DespawnActionEventHandler();

    public void Despawn()
    {
        EmitSignal(SignalName.DespawnAction);
        QueueFree();
        
    }
}
