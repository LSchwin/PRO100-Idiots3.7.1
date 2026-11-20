using Godot;
using System.Threading.Tasks;
using Direction = Action.Direction;


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

	private AnimatedSprite2D sprite;
	private Area2D hurtbox;
	private RichTextLabel stateText;
	private Timer dashGhostTimer;

	private PackedScene knifeSwingScene;
	private PackedScene throwingCardScene;
	private ThrowingCard throwingCardInst;

	private bool facingRight = true;

	private bool canDash = true;
	private bool canMove = true;
	private bool canAttack = true;
	private bool isDashing = false;
	private bool isSprinting = false;
	private float direction;
	private float sprintDir;
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
		sprite = GetNode<AnimatedSprite2D>("Model/AnimatedSprite");
		hurtbox = GetNode<Area2D>("Hurtbox");
		stateText = GetNode<RichTextLabel>("StateText");
		dashGhostTimer = GetNode<Timer>("DashGhostTimer");
		dashGhostTimer.Timeout += () => InstanceGhost();
		InstanceBaseAttacks();
    }

	public override void _PhysicsProcess(double delta)
	{
		ProcessMovement(delta);
		ProcessAtk(delta);
		ProcessAnims(delta);

		MoveAndSlide();
	}

	private void InstanceBaseAttacks()
	{
        knifeSwingScene = GD.Load<PackedScene>("uid://mksol6otni8d");
		throwingCardScene = GD.Load<PackedScene>("uid://sara4ouoatq5");
	}

	private void ProcessMovement(double delta)
	{
        Vector2 NewVelocity = Velocity;
        if (!canMove)
		{
			NewVelocity.X = 0;
			Velocity = NewVelocity;
            return;
        }

        direction = Input.GetAxis("left", "right");
		if(direction != 0)
		{
			sprintDir = direction;
		}

		if (isSprinting && !isDashing)
		{
			CurrState = States.Sprint;
			
		}
		else if (isDashing)
		{
			CurrState = States.Dash;
		}
		else
		{
            if(direction == 0)
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
			sprite.FlipH = false;
			facingRight = true;
		} else if (Velocity.X < 0)
		{
			sprite.FlipH = true;
            facingRight = false;
        }

		switch (CurrState)
		{
			case States.Dash:
				{
                    if (facingRight)
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
                NewVelocity.X = Speed * sprintDir * 2;
				break;
            case States.Walk:
                NewVelocity.X = direction * Speed;
				break;
			case States.Idle:
				NewVelocity.X = 0;
				break;

            default:
					break;
		}


		if (Input.IsActionJustPressed("jump") && IsOnFloor() && CurrState != States.Dash)
		{
			NewVelocity.Y -= JumpHeight;
		}

		if (Input.IsActionJustPressed("dash") && canDash)
		{
			Dash();
		}

		if (Input.IsActionJustReleased("dash") && isSprinting)
		{
			isSprinting = false;
		}


		if (!IsOnFloor() && !isDashing)
		{
			NewVelocity.Y += Gravity;
		}

		stateText.Text = "State: " + CurrState;


		ProcessWallCling(delta);
		SetVel(NewVelocity);
	}

	private void ProcessWallCling(double delta)
	{

	}

	private async Task ProcessAtk(double delta)
	{
		if (Input.IsActionJustPressed("knife_attack") && canAttack)
		{
			Direction AtkDir = GetPlayerDirection();
            canAttack = false;
            KnifeSwing SwingInst = knifeSwingScene.Instantiate<KnifeSwing>();
			AddChild(SwingInst);
			SwingInst.Attack(AtkDir);

            await DelaysCS.Wait(0.3f); // todo: add this value to the attack itself
			canAttack = true;
        }

		if (Input.IsActionJustPressed("throw_card_attack"))
		{
            Direction AtkDir = GetPlayerDirection(true);
            //CanAttack = false;
            ThrowingCardAttack(AtkDir);
		}
	}

	private void ProcessAnims(double delta)
	{

	}

	private void SetVel(Vector2 vel)
	{
		Velocity = vel;
	}

	private void ThrowingCardAttack(Action.Direction direction)
	{
		if(throwingCardInst == null)
		{
			throwingCardInst = throwingCardScene.Instantiate<ThrowingCard>();
            AddChild(throwingCardInst);
            
		}
		if (!throwingCardInst.OnCooldown)
		{
            throwingCardInst.Spawn(direction);
		}
	}

	private async Task Dash()
	{
		hurtbox.SetDeferred("monitoring", false);
		canDash = false;
		isDashing = true;
		dashGhostTimer.Start();
        await DelaysCS.Wait(0.2f);
        isDashing = false;
        dashGhostTimer.Stop();
        if (Input.IsActionPressed("dash"))
		{
            isSprinting = true;
        }
        hurtbox.SetDeferred("monitoring", true);
        await DelaysCS.Wait(0.2f);
		canDash = true;
    }

	private void InstanceGhost()
	{
		PackedScene scene = GD.Load<PackedScene>("uid://cpu6bpre70y1m");
        var inst = scene.Instantiate<Ghost>();
		inst.Hframes = sprite.SpriteFrames.GetFrameCount(sprite.Animation);
        inst.Frame = sprite.Frame;
		inst.FlipH = !facingRight;
		AddSibling(inst);
		inst.Position = Position;
	}

	private Direction GetPlayerDirection(bool atkWhileGrounded = false)
	{

        if ((atkWhileGrounded || !IsOnFloor()) && Input.IsActionPressed("down"))
        {
            return Direction.Down;
        }
        else if (Input.IsActionPressed("up"))
        {
            return Direction.Up;
        }
        else if (!facingRight)
        {
            return Direction.Left;
        }
        else
        {
            return Direction.Right;
        }
    }
}
