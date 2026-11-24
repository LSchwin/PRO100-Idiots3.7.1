using Godot;
using Node2DArray = Godot.Collections.Array<Godot.Node2D>;
using System;

public partial class DamageArea : Area2D
{
    public override void _Ready()
    {
        Area2D area = GetNode<Area2D>(".");
    }

    [Export]
    public ETeam Team { get; set; } = ETeam.None;


    [Export]
    public int Damage { 
        get; 
        set
        {
            if (value < 0) field = 0;
            else field = value;
        }
    }

    [Export]
    public int NumPierces { 
        get; 
        set
        {
            if (value < -1) field = -1;
            else field = value;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Node2DArray bodies = GetOverlappingBodies();
        
        foreach(Node2D body in bodies)
        {
            if (body is TileMapLayer)
            {
                GD.Print("tilemap");
                EmitSignal(SignalName.Collide);
            }
            Entity entity = body as Entity;
            if (entity is null) return;
            if (Team == ETeam.None || Team != entity.Team)
            {
                EmitSignal(SignalName.Hit);
                NumPierces = (NumPierces <= 0) ? NumPierces : NumPierces - 1;
                GD.Print($"Num Pierces : {NumPierces}");
                if (entity.TakeDamage(Damage))
                {
                    entity.Die();
                }
                if (NumPierces == 0)
                {
                    QueueFree();
                }
            }
        }

    }

    [Signal]
    public delegate void MaxPiercesReachedEventHandler();
    [Signal]
    public delegate void HitEventHandler();
    [Signal]
    public delegate void CollideEventHandler();
}
