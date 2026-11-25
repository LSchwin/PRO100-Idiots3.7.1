using Godot;
using System;
using System.Threading.Tasks;

public partial class ProjectileAction : Action
{
    [Export]
    String ProjectileUID;

    [Signal]
    public delegate void OffCooldownEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
    public override void _Ready()
    {
        if (Lifespan > 0)
        {
            base._Ready();
        }
    }

    public override void Attack(Direction direc)
    {
        base.Attack(direc);
        Spawn(direc);
    }

    public async Task Spawn(Direction direction)
    {
        if (!ProjectileUID.IsWhiteSpace())
        {
            PackedScene ProjectileScene = GD.Load<PackedScene>(ProjectileUID);
            Projectile ProjectileInst = ProjectileScene.Instantiate<Projectile>();
            ProjectileInst.Direction = direction;
            ProjectileInst.Position = GlobalPosition;
            ProjectileInst.Team = Team;
            AddChild(ProjectileInst);
        }
    }
}
