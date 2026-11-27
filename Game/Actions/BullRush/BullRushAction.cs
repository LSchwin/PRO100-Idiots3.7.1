using Godot;
using System;
using System.Threading.Tasks;

// Player exclusive

//TODO: Fix this throwing errors and not actually dealing damage
public partial class BullRushAction : Action
{
    private Player playerOwner;
    private Explosion explosion;
    private Area2D collideDetect;
    [Export]
    public int ExplosionDamage;
    private bool charging = false;

    public override void _Ready()
    {
        base._Ready();
        explosion = GetNode<Explosion>("Explosion");
        collideDetect = GetNode<Area2D>("CollideDetect");
        collideDetect.BodyEntered += (body) => Collide(body);
        explosion.SetDamage(ExplosionDamage);
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (entityOwner is Player)
        {
            playerOwner = (Player)entityOwner;
            if (chargingUp)
            {
                playerOwner.CanMove = false;
                // Do a cool charge up effect here
            }
            else if (charging)
            {
                playerOwner.CanMove = true;
                playerOwner.IsDashing = true;
                if (playerOwner.FacingRight)
                {
                    collideDetect.Position = new Vector2(13, 2);
                }
                else
                {
                    collideDetect.Position = new Vector2(-13, 2);
                }
            }
        }
    }

    private async Task Collide(Node2D body)
    {
        if(!(body is Player))
        {
            charging = false;
            explosion.Explode();
            playerOwner.CanMove = false;
            playerOwner.IsDashing = false;
            await DelaysCS.Wait(0.5f);
            playerOwner.CanMove = true;
            Despawn();
        }
    }

    protected override void ChargeUpFinish()
    {
        chargingUp = false;
        charging = true;
        lifespan.Start();
        lifespan.Timeout += Despawn;
    }

    public override void Despawn()
    {
        lifespan.Timeout -= Despawn;
        lifespan.Stop();
        playerOwner.IsDashing = false;
        base.Despawn();
    }
}