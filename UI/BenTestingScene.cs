using Godot;
using System;

public partial class BenTestingScene : Control
{
	public override void _Process(double delta)
	{
		Button button = GetNode<Button>("Button1");
		button.Position = new Vector2(LerpHelperCS.Lerp(button.Position.X, 700, 4.0f, (float)delta), button.Position.Y);
    }

	public async void OnButton1Pressed()
	{
        MusicCS.Instance.Play("Test 0");
		await DelaysCS.Instance.Wait(1.5f);
		GD.Print("delayed 1.5 seconds!");
    }
	public async void OnButton2Pressed()
	{
		MusicCS.Instance.Play("Test 1");
		await DelaysCS.Instance.Wait(3.0f);
		GD.Print("delayed 3.0 seconds!");
    }

	
	
}
