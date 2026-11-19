using Godot;
using System;

public static class LerpHelperCS
{
    // The reference to the GDScript script
    private static Script GDScriptClass() { return GD.Load<GDScript>("uid://pimd4ejjuy4s"); }

    /// <summary>
    /// Linearly interpolates between two floats, accounting for delta time.
    /// </summary>
    /// <returns>The in-between value.</returns>
	public static float Lerp(float from, float to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("lf", from, to, previousWeight, delta);
    }

    /// <summary>
    /// Linearly interpolates between two angles IN RADIANS, accounting for delta time.
    /// </summary>
    /// <returns>The in-between value.</returns>
	public static float LerpAngle(float from, float to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("la", from, to, previousWeight, delta);
    }

    /// <summary>
    /// Linearly interpolates between two Vector2s, accounting for delta time.
    /// </summary>
    /// <returns>The in-between value.</returns>
	public static float Lerp(Vector2 from, Vector2 to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("lv2", from, to, previousWeight, delta);
    }

    /// <summary>
    /// Linearly interpolates between two Vector3s, accounting for delta time.
    /// </summary>
    /// <returns>The in-between value.</returns>
	public static float Lerp(Vector3 from, Vector3 to, float previousWeight, float delta)
    {
        return (float)GDScriptClass().Call("lv3", from, to, previousWeight, delta);
    }
}
