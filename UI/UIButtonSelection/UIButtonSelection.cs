using Godot;
using Godot.Collections;

public partial class UIButtonSelection : Node
{
	[Export]
	public Array<Control> Buttons { get; set; } = new Array<Control>() { };

	/// <summary>
	/// Enables the Selector with this Node's array of buttons.
	/// </summary>
	/// <param name="vertical">Determines whether or not up/down keys are used to navigate (true) or left/right keys (false).</param>
	public void Activate(bool vertical = true)
	{
		SelectorCS.SetButtons((Array)Buttons, vertical);
	}
}
