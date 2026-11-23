using Godot;
using System;

public partial class PersistentCS : Node
{
    public static PersistentCS Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;
    }
    private Node Global() // The reference to the GDScript global
    { return GetNode("/root/Persistent"); }

    public static void SaveSettings()
    {
        Instance.Global().Call("save_settings");
    }
}
