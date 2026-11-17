using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class CardResource : Resource
{
	[Export] PackedScene action;
	[Export] Dictionary passive;
	[Export] public string name { get; set; }
	[Export] public string description { get; set; }
	[Export] Texture2D texture;
	[Export] 
	public float BaseCooldown { 
		get; 
		set { field = (value < 0.0f) ? 0.0f : value; }
	}
}
