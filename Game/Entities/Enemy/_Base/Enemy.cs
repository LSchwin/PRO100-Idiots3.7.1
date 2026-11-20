using Godot;
using System;

public partial class Enemy : Character
{
    Area2D SightArea;
    Godot.Collections.Array<PackedScene> Actions = [];
    public int EnemyScore { get; set; }

    public override void _Ready()
    {
        SightArea = GetNode<Area2D>("SightArea");
        SightArea.AreaEntered += (body) => SightAreaBodyEntered(body);
        SightArea.AreaExited += (body) => SightAreaBodyExited(body);
    }

    public override void _Process(double delta)
    {
        ProcessMovement();
        ProcessAtk();
        ProcessAnims();
    }
    private void ProcessMovement()
    {

    }

    private void ProcessAtk()
    {

    }

    private void ProcessAnims()
    {

    }


    private void SightAreaBodyEntered(Node2D body)
    {

    }

    private void SightAreaBodyExited(Node2D body)
    {

    }
    
}
