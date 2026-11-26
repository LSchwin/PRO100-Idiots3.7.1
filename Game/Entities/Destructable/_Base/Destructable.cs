using Godot;
using System;

public partial class Destructable : WorldObject
{
    private GpuParticles2D particleSpawner = null;
    private bool isDestroying = false;
    public override void _Ready()
    {
        particleSpawner = GetNode<GpuParticles2D>("ParticleSpawner");
        Destroyed += Destroy;
    }

    [Signal]
    public delegate void DestroyedEventHandler();

    public async void Destroy()
    {
        if (!isDestroying)
        {
            isDestroying = true;
            GetNode<AnimatedSprite2D>("Model/BulbSprite").Hide();
            particleSpawner.Emitting = true;
            GD.Print("Die");
            await ToSignal(particleSpawner, "finished");
            QueueFree();
        }
    }

    public override void Die()
    {
        Destroy();
    }
}
