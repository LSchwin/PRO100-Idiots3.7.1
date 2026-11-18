using Godot;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Godot.TextServer;



public partial class Player : Character
{
	[Export]
	public float Speed = 10;
	[Export]
	public float JumpHeight = 10;
	[Export]
	public float Gravity = 10;
	[Export]
	public float DashMultiplier = 3;

	private AnimatedSprite2D Sprite;
	private Area2D Hurtbox;

	private bool FacingRight = true;

	private bool CanDash = true;
	private bool IsDashing = false;
	private bool IsSprinting = false;
	private float Direction;
	private float SprintDir;

	enum States
	{

	}

	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("Model/AnimatedSprite");
		Hurtbox = GetNode<Area2D>("Hurtbox");
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
		Direction = Input.GetAxis("left", "right");
		if(Direction != 0)
		{
			SprintDir = Direction;
		}
		Vector2 NewVelocity = Velocity;

		if (IsSprinting && !IsDashing)
		{
			NewVelocity.X = Speed * SprintDir * 2;
		}
		else if (IsDashing)
		{
            if (FacingRight)
			{
				NewVelocity.X = Speed * DashMultiplier;
			}
			else
			{
				NewVelocity.X = -Speed * DashMultiplier;
			}
			NewVelocity.Y = 0;
		}
		else
		{
            NewVelocity.X = Direction * Speed;
        }

		if (Velocity.X > 0)
		{
			Sprite.FlipH = false;
			FacingRight = true;
		} else if (Velocity.X < 0)
		{
			Sprite.FlipH = true;
            FacingRight = false;
        }


		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			NewVelocity.Y -= JumpHeight;
		}

		if (Input.IsActionJustPressed("dash") && CanDash)
		{
			Dash();
		}

		if (Input.IsActionJustReleased("dash") && IsSprinting)
		{
			IsSprinting = false;
		}


		if (!IsOnFloor() && !IsDashing)
		{
			NewVelocity.Y += Gravity;
		}


		ProcessWallCling(delta);


		Velocity = NewVelocity;
	}

	private void ProcessWallCling(double delta)
	{

	}

	private void ProcessAtk(double delta)
	{

	}

	private void ProcessAnims(double delta)
	{

	}

	private async Task Dash()
	{
		Hurtbox.SetDeferred("monitoring", false);
		CanDash = false;
		IsDashing = true;
        await DelaysCS.Instance.Wait(0.2f);
        IsDashing = false;
		IsSprinting = true;
        Hurtbox.SetDeferred("monitoring", true);
        await DelaysCS.Instance.Wait(0.2f);
		CanDash = true;
    }
}
