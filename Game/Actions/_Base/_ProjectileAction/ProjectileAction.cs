using Godot;
using System;
using System.Threading.Tasks;

public partial class ProjectileAction : Action
{
    // ProjectileAction acts as a projectile spawner, which is created when the corresponding card is used.
    // It ovverides the base attack function to spawn a projectile.
    // The spawned projectile acts independently of the spawner.
    [Export]
    String ProjectileUID;
    // Set UID in godot editor

    [Signal]
    public delegate void OffCooldownEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
    public override void _Ready()
    {
        base._Ready();
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
