using Godot;
using System;



public partial class Player : Character
{
    [Export]
    public float Speed = 10;
	[Export]
	public float JumpHeight = 10;
	[Export]
	public float Gravity = 10;

	private AnimatedSprite2D sprite;

    enum States
	{

	}

    public override void _Ready()
    {
		sprite = GetNode<AnimatedSprite2D>("Model/AnimatedSprite");
    }

	public override void _Process(double delta)
	{
		ProcessMovement(delta);
		ProcessAtk(delta);
		ProcessAnims(delta);

        MoveAndSlide();
    }

	private void ProcessMovement(double delta)
	{
		float direction = Input.GetAxis("left", "right");

		Vector2 NewVelocity = Velocity;
		NewVelocity.X = direction * Speed;
		

		if (Velocity.X > 0)
		{
			sprite.FlipH = false;
		} else if (Velocity.X < 0)
		{
			sprite.FlipH = true;
		}

		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			NewVelocity.Y -= JumpHeight;
		}

		if(!IsOnFloor())
		{
			NewVelocity.Y += Gravity;
		}

		ProcessWallCling(delta);
		ProcessSprint(delta);

        Velocity = NewVelocity;
    }

	private void ProcessWallCling(double delta)
	{

	}

	private void ProcessSprint(double delta)
	{

	}

	private void ProcessAtk(double delta)
	{

	}

	private void ProcessAnims(double delta)
	{

	}
}
