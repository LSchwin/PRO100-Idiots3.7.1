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


    /// <summary>
    /// Used to await an amount in seconds.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Signal Wait(float time)
	{
		return (Signal)Instance.Global().Call("wait", time);
	}

	/// <summary>
	/// Used to await an amount in seconds, irrespective of the Engine's time scale.
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public static Signal Pause(float time)
	{
		return (Signal)Instance.Global().Call("pause", time);
	}
}
