using Godot;
using System;
using System.Threading.Tasks;

public partial class ProjectileAction : Action
{
    [Export]
    String ProjectileUID;
    [Export]
    float Cooldown;
    Timer CooldownTimer;
    public bool OnCooldown { get; set; } = false;

    [Signal]
    public delegate void OffCooldownEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
    public override void _Ready()
    {
        CooldownTimer = GetNode<Timer>("Cooldown");
        CooldownTimer.WaitTime = Cooldown;
        CooldownTimer.Timeout += () => { OnCooldown = false; EmitSignal(SignalName.OffCooldown); };
        if(Lifespan > 0)
        {
            base._Ready();
        }
    }

    public async Task Spawn(int direc)
    {
        OnCooldown = true;
        if (!ProjectileUID.IsWhiteSpace())
        {
            PackedScene ProjectileScene = GD.Load<PackedScene>(ProjectileUID);
            Projectile ProjectileInst = ProjectileScene.Instantiate<Projectile>();
            ProjectileInst.Direc = direc;
            ProjectileInst.Position = GlobalPosition;
            AddChild(ProjectileInst);
        }
        CooldownTimer.Start();
    }
}
