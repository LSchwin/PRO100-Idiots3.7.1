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


    public static void SetButtons(Array buttons, bool vertical = true)
	{
        Instance.Global().Call("set_buttons", buttons, vertical);
    }

    public static void SetButtons(Node buttonsOwner, Array buttonNames, bool vertical = true)
    {
        Array buttons = new Array() { };
        foreach (string name in buttonNames)
        {
            buttons.Add(buttonsOwner.GetNode(name));
            GD.Print(buttonsOwner.GetNode(name));
        }
        SetButtons(buttons, vertical);
    }

    public static void Disable()
	{
        Instance.Global().Call("disable");
    }
}
