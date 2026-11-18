using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

public partial class RoomScript : Node2D
{
	public enum ConnectDirection
	{
		Left = 0, 
		Right, 
		Up, 
		Down
	}

	struct Connection
	{
		public Vector2I coordinates;
		public ConnectDirection direction;
		public bool open;
	}

	public Vector2I size;
	public TileMapLayer connectionLayer;

	List<Connection> connectionsLeft;
	List<Connection> connectionsRight;
	List<Connection> connectionsUp;
	List<Connection> connectionsDown;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		connectionLayer = GetNode<TileMapLayer>("ConnectionTileMapLayer");
		size = connectionLayer.GetUsedRect().Size;
		SetConnections();
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
			if (enemy is not Enemy) continue;
			roomEnemyScore += ((Enemy)enemy).EnemyScore;
		}

		return roomEnemyScore;
	}

	public int GetLootScore()
	{
		int roomLootScore = 0;
		var enemies = GetNode<Node2D>("Loot");

		var lootArray = enemies.GetChildren();
		foreach (var loot in lootArray)
		{
            if (loot is not Pickup) continue;
			roomLootScore += ((Pickup)loot).LootScore;
        }

        return roomLootScore;
	}

	private void SetConnections()
	{
		AddConnections(ref connectionsLeft, ConnectDirection.Left, new Vector2I(0, 0));
		AddConnections(ref connectionsRight, ConnectDirection.Right, new Vector2I(1, 0));
		AddConnections(ref connectionsUp, ConnectDirection.Up, new Vector2I(2, 0));
		AddConnections(ref connectionsDown, ConnectDirection.Down, new Vector2I(3, 0));
    }

	private void AddConnections(ref List<Connection> connections, ConnectDirection direction, Vector2I id)
	{
        Godot.Collections.Array<Vector2I> connectionCoordinates = connectionLayer.GetUsedCellsById(0, id);
        foreach (Vector2I coordinate in connectionCoordinates)
        {
            Connection connection = new Connection();
            connection.direction = direction;
            connection.coordinates = coordinate;
            connection.open = false;
            connections.Add(connection);
        }
    }


}
