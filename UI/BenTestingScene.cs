using Godot;
using Godot.Collections;

public partial class BenTestingScene : Control
{
    public override void _Ready()
    {
		Array buttons = new Array() { GetNode("UIButton1"), GetNode("UIButton2"), GetNode("UIButton3") };
        SelectorCS.Instance.SetButtons(buttons);
    }

	public override void _Process(double delta)
	{
		//Control button = GetNode<Control>("UIButton1");
		//button.Position = new Vector2(LerpHelperCS.Lerp(button.Position.X, 700, 4.0f, (float)delta), button.Position.Y);
    }

	public async void OnButton1Pressed()
	{
        //MusicCS.Instance.Play("Test 0");
		await DelaysCS.Instance.Wait(1.5f);
		GD.Print("delayed 1.5 seconds!");
    }
	public async void OnButton2Pressed()
	{
		//MusicCS.Instance.Play("Test 1");
		await DelaysCS.Instance.Wait(3.0f);
		GD.Print("delayed 3.0 seconds!");
    }

	
	
}
