using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

public partial class Room : Node2D
{
	public enum ConnectDirection
	{
		Left = 0, 
		Right, 
		Up, 
		Down
	}

    public enum RoomType
    {
        Normal = 0,
		Treasure,
		Start,
		Exit,
		Shop,
		Side
    }

	public enum AvailableConnectionDirection //may actually not be able to use this
	{
		HorizontalOnly,
		VerticalOnly,
		All,
		LeftDown,
		RightDown,
		UpRight,
		UpLeft,
		NoLeft,
		NoRight,
		NoUp,
		NoDown,
	}

    public struct Connection
	{
		public Connection() {}
		public ConnectDirection direction;
        public Vector2I coordinatesLocal;
        public Godot.Vector2 coordinatesWorld;
		public bool open = false;
	}

	public static Vector2I connectTileStartLeft = new Vector2I(0, 0);
	public static Vector2I connectTileStartRight = new Vector2I(1, 0);
	public static Vector2I connectTileStartUp = new Vector2I(2, 0);
	public static Vector2I connectTileStartDown = new Vector2I(2, 1);

	[Export]
	public RoomType roomType;

	[Export]
    AvailableConnectionDirection connectionDirection;

    public ConnectDirection comingFrom;
    public ConnectDirection goingTo;
	public Vector2I size;
	public TileMapLayer connectionLayer;
	public TileMapLayer decorLayer;


    public List<List<Connection>> allConnections = new List<List<Connection>>();
    public List<Connection> connectionsLeft = new List<Connection>();
	public List<Connection> connectionsRight = new List<Connection>();
	public List<Connection> connectionsUp = new List<Connection>();
	public List<Connection> connectionsDown = new List<Connection>();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		connectionLayer = GetNode<TileMapLayer>("ConnectionTileMapLayer");
		decorLayer = GetNode<TileMapLayer>("TileMapLayer");
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
		AddConnections(ref connectionsLeft, connectTileStartLeft, ConnectDirection.Left);
		AddConnections(ref connectionsRight, connectTileStartRight, ConnectDirection.Right);
		AddConnections(ref connectionsUp, connectTileStartUp, ConnectDirection.Up);
		AddConnections(ref connectionsDown, connectTileStartDown, ConnectDirection.Right);

		allConnections.Add(connectionsLeft);
		allConnections.Add(connectionsRight);
		allConnections.Add(connectionsUp);
		allConnections.Add(connectionsDown);
    }

    private void AddConnections(ref List<Connection> connections, Vector2I id, ConnectDirection direction)
	{
        Godot.Collections.Array<Vector2I> connectionCoordinates = connectionLayer.GetUsedCellsById(0, id);
        foreach (Vector2I coordinate in connectionCoordinates)
        {
            Connection connection = new Connection();
            connection.coordinatesLocal = coordinate;
			connection.coordinatesWorld = connectionLayer.ToGlobal(coordinate);
			connection.direction = direction;

            connection.open = true; // TODO: Remove this line after room generation

            connections.Add(connection);
        }
    }

	public List<Connection> GetAvailableConnections()
	{
		List<Connection> avaiableConnections = new List<Connection>();

		foreach(var list in allConnections)
		{
			foreach(var connection in list)
			{
				if (connection.open) avaiableConnections.Add(connection);
            }
		}

		return avaiableConnections;
    }

    public List<Connection> GetAvailableConnections(List<Connection> connections)
    {
        List<Connection> avaiableConnections = new List<Connection>();

        foreach (var connection in connections)
        {
            if (connection.open) avaiableConnections.Add(connection);
        }

        return avaiableConnections;
    }


}
