using Godot;
using System;
using System.Threading.Tasks;

public partial class ProjectileAction : Action
{
    [Export]
    String ProjectileUID;
    [Export]
    public float Cooldown;
    private Timer cooldownTimer;
    public bool OnCooldown { get; set; } = false;

    [Signal]
    public delegate void OffCooldownEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
    public override void _Ready()
    {
        cooldownTimer = GetNode<Timer>("Cooldown");
        cooldownTimer.WaitTime = Cooldown;
        cooldownTimer.Timeout += () => { OnCooldown = false; EmitSignal(SignalName.OffCooldown); };
        if(Lifespan > 0)
        {
            base._Ready();
        }
    }

    public async Task Spawn(Direction direction)
    {
        OnCooldown = true;
        if (!ProjectileUID.IsWhiteSpace())
        {
            PackedScene ProjectileScene = GD.Load<PackedScene>(ProjectileUID);
            Projectile ProjectileInst = ProjectileScene.Instantiate<Projectile>();
            ProjectileInst.Direction = direction;
            ProjectileInst.Position = GlobalPosition;
            AddChild(ProjectileInst);
        }
        cooldownTimer.Start();
    }
}
