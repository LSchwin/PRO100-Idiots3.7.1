using Godot;



public partial class DelaysCS : Node
{
	public static DelaysCS Instance { get; private set; }


	public override void _Ready()
	{
		Instance = this;
	}

	private Node Global()
    { return GetNode("/root/Delays"); }

    public Signal Wait(float time)
	{
		return (Signal)Global().Call("wait", time);
	}
	public Signal Pause(float time)
	{
		return (Signal)Global().Call("pause", time);
	}
}
