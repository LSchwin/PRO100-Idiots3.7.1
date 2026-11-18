using Godot;
using Godot.Collections;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SelectorCS.SetButtons(GetNode<Control>("Buttons"), new Array(){"BtnStart", "BtnSettings", "BtnQuit"});
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
		GetTree().Quit();
	}
}
