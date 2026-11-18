using Godot;
using System;

public partial class Destructable : WorldObject
{
    private GpuParticles2D particleSpawner = null; 
    public override void _Ready()
    {
        particleSpawner = GetNode<GpuParticles2D>("ParticleSpawner");
        Destroyed += Destroy;
    }

    [Signal]
    public delegate void DestroyedEventHandler();

    public async void Destroy()
    {
        GetNode<AnimatedSprite2D>("Model/BulbSprite").Hide();
        particleSpawner.Emitting = true;
        await ToSignal(particleSpawner, "finished");
        QueueFree();
    }
}
