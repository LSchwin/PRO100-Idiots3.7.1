using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using static Room;
using BranchRoomTypeArray = Godot.Collections.Array<Godot.Collections.Array<Room.RoomType>>;
using Connection = Room.Connection;
using PackedSceneArray = Godot.Collections.Array<Godot.PackedScene>;
using RoomDictionary = Godot.Collections.Dictionary<Room.RoomType, Godot.Collections.Array<Godot.PackedScene>>;
using RoomType = Room.RoomType;
using RoomTypeArray = Godot.Collections.Array<Room.RoomType>;


public partial class Level : Node2D
{

    /// <summary>
    /// This is the order the rooms appear on the main branch.
    /// </summary>
    [Export]
    public RoomTypeArray roomOrder = new RoomTypeArray();

    /// <summary>
    /// This is the order the rooms appear on the main branch.
    /// </summary>
    [Export]
    public BranchRoomTypeArray branchRoomOrders = new BranchRoomTypeArray();

    /// <summary>
    /// This is the types of enemies that can appear in the level.
    /// </summary>
    [Export]
    public PackedSceneArray spawnableEnemies = new PackedSceneArray();

    /// <summary>
    /// This is the types of roomcs that can appear in the level.
    /// </summary>
    [Export]
    public RoomDictionary AvailableRooms = new RoomDictionary()
    {
        {RoomType.Normal,   new PackedSceneArray() },
        {RoomType.Treasure, new PackedSceneArray() },
        {RoomType.Start,    new PackedSceneArray() },
        {RoomType.Exit,     new PackedSceneArray() },
        {RoomType.Shop,     new PackedSceneArray() },
        {RoomType.Side,     new PackedSceneArray() }
    };

    public List<Room> allUsedRooms = new List<Room>();

    public int enemyScore { get; set; }
    public int lootScore { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        var room5 = GetNode<Room>("Room5");
        //room5.comingFrom = Room.ConnectDirection.Up;
        room5.goingTo = Room.ConnectDirection.Down;

        var room4 = GetNode<Room>("Room4");
        room4.comingFrom = Room.ConnectDirection.Up;
        room4.goingTo = Room.ConnectDirection.Right;

        DeleteConnection(room5, room4); 

        var room1 = GetNode<Room>("Room1");
		room1.comingFrom = Room.ConnectDirection.Left;
		room1.goingTo = Room.ConnectDirection.Right;

        DeleteConnection(room4, room1); 

		var room2 = GetNode<Room>("Room2");
        room2.comingFrom = Room.ConnectDirection.Left;
        room2.goingTo = Room.ConnectDirection.Down;

        DeleteConnection(room1, room2);

        var room3 = GetNode<Room>("Room3");
        room3.comingFrom = Room.ConnectDirection.Up;
        room3.goingTo = Room.ConnectDirection.Down;

        DeleteConnection(room2, room3);

        

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Generate()
	{

	}

    public void Populate()
    {

    }

	private void SpawnEnemies()
	{

	}

    private void SpawnLoot()
    {

    }

	private void DeleteConnection(Room comingFromRoom, Room goingToRoom)
	{
		switch(comingFromRoom.goingTo)
		{
			case Room.ConnectDirection.Left:
                EraseConnection(comingFromRoom, comingFromRoom.connectionsLeft, new Vector2I(0, 1));
                EraseConnection(goingToRoom, goingToRoom.connectionsRight, new Vector2I(0, 1));
                break;

            case Room.ConnectDirection.Right:
                EraseConnection(comingFromRoom, comingFromRoom.connectionsRight, new Vector2I(0, 1));
                EraseConnection(goingToRoom, goingToRoom.connectionsLeft, new Vector2I(0, 1));
                break;

            case Room.ConnectDirection.Up:
                EraseConnection(comingFromRoom, comingFromRoom.connectionsUp, new Vector2I(1, 0));
                EraseConnection(goingToRoom, goingToRoom.connectionsDown, new Vector2I(1, 0));
                break;

            case Room.ConnectDirection.Down:
                EraseConnection(comingFromRoom, comingFromRoom.connectionsDown, new Vector2I(1, 0));
                EraseConnection(goingToRoom, goingToRoom.connectionsUp, new Vector2I(1, 0));
                break;
        }
    }

    private void EraseConnection(Room room, List<Connection> connections, Vector2I tileCoordinates)
    {
        foreach (Room.Connection connection in connections)
        {
            if (!connection.open) continue;
            room.decorLayer.EraseCell(connection.coordinates - tileCoordinates);
            room.decorLayer.EraseCell(connection.coordinates + tileCoordinates);
            room.decorLayer.EraseCell(connection.coordinates);
            room.connectionLayer.EraseCell(connection.coordinates);

            //var num = room.connectionLayer.ToGlobal(connection.coordinates);
            //var num2 = room.connectionLayer.ToLocal(num);
            //
            //GD.Print(connection.coordinates);
            //GD.Print(num);
            //GD.Print(num2);
        }

        
    }
}
