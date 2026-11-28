using Godot;
using System;

public partial class Explosion : Node2D
{
    [Export]
    public float Lifespan = 1.0f;
    [Export]
    public float ExplosionSize = 1.0f;
    public int ExplosionDamage;
    private Timer lifespan;
    private DamageArea damage;
    private CollisionShape2D collisionShape;
    private CircleShape2D explosionCollision;

    [Signal]
    public delegate void ExplodeFinishEventHandler();

    public override void _Ready()
    {
        lifespan = GetNode<Timer>("Lifespan");
        damage = GetNode<DamageArea>("Damage");
        collisionShape = GetNode<CollisionShape2D>("Damage/CollisionShape2D");
        explosionCollision = (CircleShape2D)collisionShape.Shape;
        explosionCollision.Radius = ExplosionSize;
        collisionShape.SetDeferred("disabled", true);
        lifespan.WaitTime = Lifespan;
        lifespan.Timeout += Despawn;
    }

    public void Explode()
    {
        // Start the explosion
        lifespan.Start();
        collisionShape.SetDeferred("disabled", false);
    }

    private void Despawn()
    {
        // Ends the explosion and emits a the ExplodeFinish signal, triggers on Lifespan timeout
        lifespan.Timeout -= Despawn;
        collisionShape.SetDeferred("disabled", true);
        EmitSignal(SignalName.ExplodeFinish);
        QueueFree();
    }

    public void SetDamage(int damage)
    {
        ExplosionDamage = damage;
        this.damage.Damage = ExplosionDamage;
    }
}
