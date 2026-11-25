using Godot;
using System;

public partial class FireworkProjectile : Projectile
{
    private Explosion explosion;

    public override void _Ready()
    {
        base._Ready();
        explosion = GetNode<Explosion>("Explosion");
    }
    public override void Collide()
    {
        CanMove = false;
        explosion.Explode();
        //base.Collide();
    }
    public override void Damage()
    {
        CanMove = false;
        explosion.Explode();
        //base.Damage();
    }
}
