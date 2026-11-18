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
    public Signal Wait(float time)
	{
		return (Signal)Global().Call("wait", time);
	}

	/// <summary>
	/// Used to await an amount in seconds, irrespective of the Engine's time scale.
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public Signal Pause(float time)
	{
		return (Signal)Global().Call("pause", time);
	}
}
