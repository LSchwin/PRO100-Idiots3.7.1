using Godot;
using Godot.Collections;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<UIButtonSelection>("Buttons").Activate();
	}

    public void OnBtnStartPressed()
	{
		ScenesCS.Swap("Testing Menu");
	}

    public void OnBtnSettingsPressed()
	{
		ScenesCS.Swap("Settings");
	}

    public void OnBtnQuitPressed()
	{
		PersistentCS.SaveSettings();
		GetTree().Quit();
	}
}
