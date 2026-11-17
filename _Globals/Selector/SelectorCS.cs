using Godot;
using Godot.Collections;

public partial class SelectorCS : Node
{
    public static SelectorCS Instance { get; private set; }
    public override void _Ready()
	{
		Instance = this;
	}

    private Node Global()
    { return GetNode("/root/Selector"); }

    public void SetButtons(Array buttons, bool vertical = true)
	{
        Global().Call("set_buttons", buttons, vertical);
    }
    public void Disable()
	{
        Global().Call("disable");
    }
}
