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
	private Timer jumpBufferTimer;
	private Timer coyoteTimeTimer;
	private Timer jumpCooldownTimer;

	private PackedScene knifeSwingScene;
	private PackedScene throwingCardScene;

	private bool facingRight = true;

	private bool canDash = true;
	private bool canMove = true;
	private bool canJump = true;
	private bool willJump = false;
	private bool canAttack = true;
	private bool canThrow = true;
	private bool isDashing = false;
	private bool isSprinting = false;
	private bool isJumping = false;
	private bool jumpOffCooldown = true;
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
		jumpBufferTimer = GetNode<Timer>("JumpTimers/JumpBuffer");
		coyoteTimeTimer = GetNode<Timer>("JumpTimers/CoyoteTime");
		jumpCooldownTimer = GetNode<Timer>("JumpTimers/JumpCooldown");
		dashGhostTimer.Timeout += () => InstanceGhost();
		coyoteTimeTimer.Timeout += () => canJump = false;
        jumpBufferTimer.Timeout += () => willJump = false;
		jumpCooldownTimer.Timeout += () => jumpOffCooldown = true;
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

		if (Input.IsActionJustPressed("dash") && canDash)
		{
			Dash();
		}

		if (Input.IsActionJustReleased("dash") && isSprinting)
		{
			isSprinting = false;
		}

		stateText.Text = "State: " + CurrState;


		NewVelocity = ProcessJump(delta, NewVelocity);
		ProcessWallCling(delta);
		SetVel(NewVelocity);
	}

	private Vector2 ProcessJump(double delta, Vector2 NewVelocity)
	{ 
		if (!IsOnFloor() && !isDashing)
		{
			NewVelocity.Y += Gravity;
		}

		// Coyote time
		if (!canJump && IsOnFloor())
		{
			canJump = true;
			if (!coyoteTimeTimer.IsStopped())
			{
				coyoteTimeTimer.Stop();
			}
		}

		if (!IsOnFloor() && canJump && coyoteTimeTimer.IsStopped())
		{
			coyoteTimeTimer.Start();
		}

		// Normal jump
		if (Input.IsActionJustPressed("jump") && jumpOffCooldown && CurrState != States.Dash && (IsOnFloor() || canJump)) 
		{
			isJumping = true;
			NewVelocity.Y = 0;
            NewVelocity.Y -= JumpHeight;
			canJump = false;
            jumpCooldownTimer.Start();
        }
		
		// Jump buffer
        if (Input.IsActionJustPressed("jump") && CurrState != States.Dash && !IsOnFloor())
        {
            willJump = true;
			jumpBufferTimer.Start();
			jumpCooldownTimer.Start();
        }

		if (willJump && IsOnFloor())
		{
			NewVelocity.Y -= JumpHeight;
		}

        if (IsOnFloor())
        {
            willJump = false;
			isJumping = false;
            jumpBufferTimer.Stop();
        }

		// Variable jump height
		if (Input.IsActionJustReleased("jump") && NewVelocity.Y < 0)
		{
			while(NewVelocity.Y < 0)
			{
				NewVelocity.Y += Gravity;
			}
		}

        return NewVelocity;
    }

	private void ProcessWallCling(double delta)
	{

	}

	private async Task ProcessAtk(double delta)
	{
        Direction AtkDir = GetPlayerDirection();

        if (Input.IsActionJustPressed("knife_attack") && canAttack)
		{
			await KnifeAttack(AtkDir);
        }

		if (Input.IsActionJustPressed("throw_card_attack") && canThrow)
		{
			GD.Print("supposed to throw a card");
            await ThrowingCardAttack(AtkDir);
		}

		if (Input.IsActionJustPressed("card_1")) { await SpecialAttack(1); }
		if (Input.IsActionJustPressed("card_2")) { await SpecialAttack(2); }
		if (Input.IsActionJustPressed("card_3")) { await SpecialAttack(3); }

		if (Input.IsActionJustPressed("test_card_swap")) 
		{ 
			if (GameCS.Instance.hand.Count == 1) 
				GameCS.RemoveFromHand((CardResource) GameCS.Instance.hand[0]);
			else 
				GameCS.Instance.MoveToHand((CardResource)GameCS.Instance.deck[0]);
        }
	}

	

	private void ProcessAnims(double delta)
	{

	}

	private void SetVel(Vector2 vel)
	{
		Velocity = vel;
	}

    /// <summary>
    /// hi
    /// </summary>
    /// <param name="slot">a number between 1 and 3 corresponding to the slot the card is in</param>
    private async Task SpecialAttack(int slot)
	{
		if (GameCS.Instance.hand.Count < slot) return;
		CardResource card = (CardResource) GameCS.Instance.hand[slot - 1];
		if (card.GetAction() == null) return;

		if (!card.OnCooldown)
		{
			Direction atkDir = GetPlayerDirection();
			card.OnCooldown = true;
			Action atkInst = card.GetAction().Instantiate<Action>();
			AddChild(atkInst);
			atkInst.Attack(atkDir);
			await DelaysCS.Wait(card.BaseCooldown);
			card.OnCooldown = false;
		}
    }

	private async Task KnifeAttack(Direction direction)
	{
        canAttack = false;
        KnifeSwing SwingInst = knifeSwingScene.Instantiate<KnifeSwing>();
        AddChild(SwingInst);
        SwingInst.Attack(direction);

        await DelaysCS.Wait(GameCS.Instance.AtkCooldown);
        canAttack = true;
    }

	private async Task ThrowingCardAttack(Direction direction)
	{
        canThrow = false;
        ThrowingCard throwingCardInst = throwingCardScene.Instantiate<ThrowingCard>();
        AddChild(throwingCardInst);
        throwingCardInst.Attack(direction);
		GD.Print("Card thrown");

        await DelaysCS.Wait(GameCS.Instance.AtkCooldown);
        canThrow = true;    
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
