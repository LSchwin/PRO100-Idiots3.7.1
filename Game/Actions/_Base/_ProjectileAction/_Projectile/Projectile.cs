using Godot;
using Direction = Action.Direction;

public partial class Projectile : CharacterBody2D
{
    [Export]
    public int MoveSpeed;
    [Export]
    public bool CanDespawn = true;
    [Export]
    public float Duration = 1.0f;

    public Direction Direction;
    private Timer lifespan; 
    private DamageArea damageArea;
    public ETeam Team { get; set; }
    // Team is set by the ProjectileAction that spawns the projectile. No need to change here.
    public bool CanMove = true;

    public override void _Ready()
    {
        base._Ready();
        lifespan = GetNode<Timer>("Lifespan");
        damageArea = GetNode<DamageArea>("DamageArea");
        damageArea.Team = Team;
        damageArea.Hit += Damage;
        damageArea.Collide += Collide;
        if (CanDespawn)
        {
            lifespan.Timeout += () => QueueFree();
            lifespan.Start();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Automatically moves in the direction set
        // If it's not moving make sure MoveSpeed isn't 0
        if (!CanMove) { return; }
        Vector2 NewVel = Velocity;
        switch (Direction)
        {
            case Direction.Left:
                Rotation = float.Pi;
                NewVel.X = -MoveSpeed;
                break;
            case Direction.Up:
                Rotation = (3* float.Pi)/2;
                NewVel.Y = -MoveSpeed;
                break;
            case Direction.Right:
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

    public virtual void Damage()
    {
        // Hit an entity
        damageArea.Hit -= Damage;
        QueueFree();
    }

    public virtual void Collide()
    {
        // Hit terrain
        damageArea.Collide -= Collide;
        QueueFree();
    }
}
