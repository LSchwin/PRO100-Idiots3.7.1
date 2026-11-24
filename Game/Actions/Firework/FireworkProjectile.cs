using Godot;
using System;

public partial class FireworkProjectile : Projectile
{
    public override void Collide()
    {
        // spawn explosion
        base.Collide();
    }

    public override void Damage()
    {
        // spawn explosion
        base.Damage();
    }
}
