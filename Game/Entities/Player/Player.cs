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
	private RichTextLabel StateText;

	private bool FacingRight = true;

	private bool CanDash = true;
	private bool IsDashing = false;
	private bool IsSprinting = false;
	private float Direction;
	private float SprintDir;
	States CurrState = States.Idle;

	enum States
	{
		Idle,
		Walk,
		Sprint,
		Dash
	}

	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("Model/AnimatedSprite");
		Hurtbox = GetNode<Area2D>("Hurtbox");
		StateText = GetNode<RichTextLabel>("StateText");

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
			CurrState = States.Sprint;
			
		}
		else if (IsDashing)
		{
			CurrState = States.Dash;
		}
		else
		{
            if(Direction == 0)
			{
				CurrState = States.Idle;
			}
			else
			{
				CurrState = States.Walk;
            }
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

		switch (CurrState)
		{
			case States.Dash:
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
					break;
                }
			case States.Sprint:
                NewVelocity.X = Speed * SprintDir * 2;
				break;
            case States.Walk:
                NewVelocity.X = Direction * Speed;
				break;
			case States.Idle:
				NewVelocity.X = 0;
				break;

            default:
					break;
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

		StateText.Text = "State:" + CurrState;


		ProcessWallCling(delta);
		SetVel(NewVelocity);
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

	private void SetVel(Vector2 vel)
	{
		Velocity = vel;
	}

	private async Task Dash()
	{
		Hurtbox.SetDeferred("monitoring", false);
		CanDash = false;
		IsDashing = true;
        await DelaysCS.Instance.Wait(0.2f);
        IsDashing = false;
		if (Input.IsActionPressed("dash"))
		{
            IsSprinting = true;
        }
        Hurtbox.SetDeferred("monitoring", true);
        await DelaysCS.Instance.Wait(0.2f);
		CanDash = true;
    }
}
