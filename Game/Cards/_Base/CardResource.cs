using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class CardResource : Resource
{
	[Export] public PackedScene Action = null;
	[Export] public Dictionary Passive = new Dictionary();
	[Export] public string Name { get; set; }
	[Export] public string Description { get; set; }
	[Export] Texture2D Texture;
	[Export] 
	public float BaseCooldown { 
		get; 
		set { field = (value < 0.0f) ? 0.0f : value; }
	}
    [Export]
    public bool OnCooldown { get; set; }
    public Dictionary GetPassives() { return Passive; }
	public PackedScene GetAction() { return Action; }
}
