using Godot;
using System;

public partial class Player : CharacterBody2D
{
	enum States
	{

	}

	public override void _Process(double delta)
	{
		ProcessMovement();
		ProcessAtk();
		ProcessAnims();
	}

	private void ProcessMovement()
	{
		ProcessWallCling();
		ProcessSprint();

    }

	private void ProcessWallCling()
	{

	}

	private void ProcessSprint()
	{

	}

	private void ProcessAtk()
	{

	}

	private void ProcessAnims()
	{

	}
}
