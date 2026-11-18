using Godot;
using System;

public partial class MusicCS : Node
{
    public static MusicCS Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;
    }
    private Node Global()
    { return GetNode("/root/Music"); }


    public static void Play(string track, float trackFadeIn = 1.0f, float previousTrackFadeOut = 1.0f)
    {
        Instance.Global().Call("play", track, trackFadeIn, previousTrackFadeOut);
    }

    public static void Stop(float fadeOut = 1.0f)
    {
        Instance.Global().Call("stop", fadeOut);
    }

    public static void SetPitch(float pitch = 1.0f, float time = 0.25f)
    {
        Instance.Global().Call("set_pitch", pitch, time);
    }

    public static void SetFilter(int cutoffHZ = 20500, float time = 0.1f)
    {
        Instance.Global().Call("set_filter", cutoffHZ, time);
    }
}
