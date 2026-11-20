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
	private Timer DashGhostTimer;

	private PackedScene KnifeSwingScene;
	private PackedScene ThrowingCardScene;
	private ThrowingCard ThrowingCardInst;

	private bool FacingRight = true;

	private bool CanDash = true;
	private bool CanMove = true;
	private bool CanAttack = true;
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
		DashGhostTimer = GetNode<Timer>("DashGhostTimer");
		DashGhostTimer.Timeout += () => InstanceGhost();
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
        KnifeSwingScene = GD.Load<PackedScene>("uid://mksol6otni8d");
		ThrowingCardScene = GD.Load<PackedScene>("uid://sara4ouoatq5");
	}

	private void ProcessMovement(double delta)
	{
        Vector2 NewVelocity = Velocity;
        if (!CanMove)
		{
			NewVelocity.X = 0;
			Velocity = NewVelocity;
            return;
        }

		Direction = Input.GetAxis("left", "right");
		if(Direction != 0)
		{
			SprintDir = Direction;
		}

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


		if (Input.IsActionJustPressed("jump") && IsOnFloor() && CurrState != States.Dash)
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

		StateText.Text = "State: " + CurrState;


		ProcessWallCling(delta);
		SetVel(NewVelocity);
	}

	private void ProcessWallCling(double delta)
	{

	}

	private async Task ProcessAtk(double delta)
	{
		if (Input.IsActionJustPressed("knife_attack") && CanAttack)
		{
			int AtkDir = 1;
			CanAttack = false;
			// 1=Left,2=Up,3=Right,4=Down 
			if(!IsOnFloor() && Input.IsActionPressed("down"))
			{
				AtkDir = 4;
			}else if (Input.IsActionPressed("up"))
			{
                AtkDir = 2;
			}else if (!FacingRight)
			{
                AtkDir = 1;
			}
			else
			{
                AtkDir = 3;
			}
			KnifeAttack(AtkDir);
            await DelaysCS.Wait(0.3f);
			CanAttack = true;
        }

		if (Input.IsActionJustPressed("throw_card_attack"))
		{
            int AtkDir = 1;
            CanAttack = false;
            // 1=Left,2=Up,3=Right,4=Down 
            if (Input.IsActionPressed("down"))
            {
                AtkDir = 4;
            }
            else if (Input.IsActionPressed("up"))
            {
                AtkDir = 2;
            }
            else if (!FacingRight)
            {
                AtkDir = 1;
            }
            else
            {
                AtkDir = 3;
            }
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

	private void KnifeAttack(int direc)
	{
        KnifeSwing SwingInst = KnifeSwingScene.Instantiate<KnifeSwing>();
		SwingInst.DespawnAction += () => AttackFinish(SwingInst);
		Vector2 SwingPos = SwingInst.Position;
		switch (direc)
		{
			case 1:
				SwingInst.Rotation = float.Pi;
				SwingPos.X -= 16;
				break;
			case 2:
                SwingInst.Rotation = (3*float.Pi)/2;
                SwingPos.Y -= 16;
                break;
			case 4:
                SwingInst.Rotation = float.Pi/2;
                SwingPos.Y += 16;
                break;
			default:
                SwingInst.Rotation = 0;
                SwingPos.X += 16;
                break;
		}
		SwingInst.Position = SwingPos;

        AddChild(SwingInst);
    }

	private void ThrowingCardAttack(int direc)
	{
		if(ThrowingCardInst == null)
		{
			ThrowingCardInst = ThrowingCardScene.Instantiate<ThrowingCard>();
            AddChild(ThrowingCardInst);
            
		}
		if (!ThrowingCardInst.OnCooldown)
		{
            GD.Print("Spawn");
            ThrowingCardInst.Spawn(direc);
		}
	}

	private void AttackFinish(Action attack)
	{
		attack.DespawnAction -= () => AttackFinish(attack);
	}

	private async Task Dash()
	{
		Hurtbox.SetDeferred("monitoring", false);
		CanDash = false;
		IsDashing = true;
		DashGhostTimer.Start();
        await DelaysCS.Wait(0.2f);
        IsDashing = false;
        DashGhostTimer.Stop();
        if (Input.IsActionPressed("dash"))
		{
            IsSprinting = true;
        }
        Hurtbox.SetDeferred("monitoring", true);
        await DelaysCS.Wait(0.2f);
		CanDash = true;
    }

	private void InstanceGhost()
	{
		PackedScene scene = GD.Load<PackedScene>("uid://baojxacg276wf");
        var inst = scene.Instantiate<Ghost>();
		inst.Hframes = Sprite.SpriteFrames.GetFrameCount(Sprite.Animation);
        inst.Frame = Sprite.Frame;
		inst.FlipH = !FacingRight;
		AddSibling(inst);
		inst.Position = Position;
	}
}
