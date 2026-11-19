using Godot;
using System;

public partial class Entity : CharacterBody2D
{
	[Export]
	public ETeam Team { get; set; } = ETeam.None;

    [Export]
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

	/// <returns>True if the entity dies due to taking damage, false otherwise</returns>
	public bool TakeDamage(int damage)
	{
		Health -= damage;
		return Health == 0;
	}

}
