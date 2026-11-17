using Godot;
using System;

public partial class Action : Node
{
    private Entity owner = null;

    public override void _Ready()
    {
        DespawnAction += Despawn;
    }

    public Entity GetActionOwner() { return owner; }
    public void SetActionOwner(Entity newOwner) { owner = newOwner; }
    public float Lifespan { 
        get;
        set
        {
            if (value < 0.0f) field = 0.0f;
            else field = value;
        } 
    }

    [Signal]
    public delegate void DespawnActionEventHandler();

    public void Despawn()
    {
        QueueFree();
    }
}
