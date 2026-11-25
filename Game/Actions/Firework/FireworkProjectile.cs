using Godot;
using System;

public partial class FireworkProjectile : Projectile
{
    private Explosion explosion;
    // Make an actual explosion effect

    public override void _Ready()
    {
        base._Ready();
        explosion = GetNode<Explosion>("Explosion");
    }
    public override void Collide()
    {
        CanMove = false;
        explosion.Explode();
    }
    public override void Damage()
    {
        CanMove = false;
        explosion.Explode();
    }
}
