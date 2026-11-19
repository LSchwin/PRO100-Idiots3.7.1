using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class CardResource : Resource
{
	[Export] PackedScene Action;
	[Export] Dictionary Passive;
	[Export] public string Name { get; set; }
	[Export] public string Description { get; set; }
	[Export] Texture2D Texture;
	[Export] 
	public float BaseCooldown { 
		get; 
		set { field = (value < 0.0f) ? 0.0f : value; }
	}
	public Dictionary GetPassives() { return Passive; }
}
