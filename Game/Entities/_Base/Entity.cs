using Godot;
using System;

public partial class Entity : CharacterBody2D
{
	public int Health { 
		get;
		set
		{
			if (value < 0)
			{
				field = 0;
			}
			else
			{
				field = value;
			}
		} 
	}

	Godot.Collections.Array<PackedScene> drops = [];    


}
