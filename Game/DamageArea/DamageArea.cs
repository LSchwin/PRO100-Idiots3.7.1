using Godot;
using System;

public partial class DamageArea : Area2D
{
    public override void _Ready()
    {
        Area2D area = GetNode<Area2D>(".");
        area.BodyEntered += OnBodyEntered;
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

    private void OnBodyEntered(Node2D body)
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
            if (entity.TakeDamage(Damage))
            {
                // To be implemented at a later date vvvvvvvvvv
                // areaParent.Die();
                // For now,
                if ((entity as Destructable) is not null)
                {
                    (entity as Destructable).Destroy();
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
