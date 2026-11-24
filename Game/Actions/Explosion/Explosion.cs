using Godot;
using System;

public partial class Explosion : Node2D
{
    [Export]
    public float Lifespan = 1.0f;
    [Export]
    public float ExplosionSize = 1.0f;
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
        collisionShape.Disabled = true;
        lifespan.WaitTime = Lifespan;
        lifespan.Timeout += Despawn;
    }

    public void Explode()
    {
        lifespan.Start();
        collisionShape.Disabled = false;
    }

    private void Despawn()
    {
        lifespan.Timeout -= Despawn;
        collisionShape.Disabled = true;
        EmitSignal(SignalName.ExplodeFinish);
        QueueFree();
    }
}
