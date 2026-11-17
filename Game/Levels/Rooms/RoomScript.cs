using Godot;
using System;

public partial class RoomScript : Node2D
{
	Vector2I size;
	TileMapLayer connectionLayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		connectionLayer = GetNode<TileMapLayer>("ConnectionTileMapLayer");
		size = connectionLayer.GetUsedRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public int GetEnemyScore()
	{
		int roomEnemyScore = 0;
		var enemies = GetNode<Node2D>("Enemies");

		var enemyArray = enemies.GetChildren();
		foreach (var enemy in enemyArray)
		{
			//roomScore += enemy.score;
		}

		return roomEnemyScore;
	}

	public int GetLootScore()
	{
		int roomLootScore = 0;
		var enemies = GetNode<Node2D>("Enemies");

		var lootArray = enemies.GetChildren();
		foreach (var loot in lootArray)
		{
            //roomLootScore += loot.score;
        }

        return roomLootScore;
	}


}
