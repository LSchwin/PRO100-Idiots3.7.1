using Godot;
using System;

public partial class FireworkProjectile : Projectile
{
    private Explosion explosion;
    [Export]
    public int damage;
    // Make an actual explosion effect

    public override void _Ready()
    {
        base._Ready();
        explosion = GetNode<Explosion>("Explosion");
        explosion.SetDamage(damage);
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
