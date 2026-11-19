using Godot;
using Godot.Collections;

public partial class UIButtonSelection : Node
{
	[Export]
	public Array<Control> Buttons { get; set; } = new Array<Control>() { };

	public void Activate(bool vertical = true)
	{
		SelectorCS.SetButtons((Array)Buttons, vertical);
	}
}
