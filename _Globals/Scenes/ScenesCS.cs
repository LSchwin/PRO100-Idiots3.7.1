using Godot;
using System;

public partial class ScenesCS : Node
{
    public static ScenesCS Instance { get; private set; }
    public override void _Ready()
	{
		Instance = this;
	}
    private Node Global()
    { return GetNode("/root/Scenes"); }


	public static void Swap(string scene, float fadeIn = 0.25f, float fadeOut = 0.25f)
	{
		Instance.Global().Call("swap", scene, fadeIn, fadeOut);
	}

	public static void FadeIn(float time = 0.25f)
	{
        Instance.Global().Call("fade_in", time);
    }
	public static void FadeOut(float time = 0.25f)
	{
        Instance.Global().Call("fade_out", time);
    }
}
