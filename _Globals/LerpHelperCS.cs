using Godot;
using System;

public static class LerpHelperCS
{
    private static Script GDScriptClass() { return GD.Load<GDScript>("uid://pimd4ejjuy4s"); }

	public static float Lerp(float from, float to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("l", from, to, previousWeight, delta);
    }
	public static float LerpAngle(float from, float to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("la", from, to, previousWeight, delta);
    }
	public static float Lerp(Vector2 from, Vector2 to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("lv2", from, to, previousWeight, delta);
    }
	public static float Lerp(Vector3 from, Vector3 to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("lv3", from, to, previousWeight, delta);
    }
}
