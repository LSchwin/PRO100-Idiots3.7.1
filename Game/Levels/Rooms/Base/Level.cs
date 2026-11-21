using Godot;
using System;
using System.Collections.Generic;

using Godot.Collections;

using RoomType = Room.RoomType;
using Connection = Room.Connection;
using RoomTypeArray = Godot.Collections.Array<Room.RoomType>;
using BranchRoomTypeArray = Godot.Collections.Array<Godot.Collections.Array<Room.RoomType>>;
using PackedSceneArray = Godot.Collections.Array<Godot.PackedScene>;
using RoomDictionary = Godot.Collections.Dictionary<Room.RoomType, Godot.Collections.Array<Godot.PackedScene>>;


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
                foreach (Room.Connection connection in comingFromRoom.connectionsLeft)
                {
                    if (!connection.open) continue;
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(0, 1));
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(0, 1));
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates);
                    comingFromRoom.connectionLayer.EraseCell(connection.coordinates);
                }
                foreach (Room.Connection connection in goingToRoom.connectionsRight)
                {
                    if (!connection.open) continue;
                    goingToRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(0, 1));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(0, 1));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates);
                    goingToRoom.connectionLayer.EraseCell(connection.coordinates);
                }
                break;

            case Room.ConnectDirection.Right:
				foreach(Room.Connection connection in comingFromRoom.connectionsRight)
				{
					if (!connection.open) continue;
					comingFromRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(0, 1));
					comingFromRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(0, 1));
					comingFromRoom.decorLayer.EraseCell(connection.coordinates);
					comingFromRoom.connectionLayer.EraseCell(connection.coordinates);
				}
                foreach (Room.Connection connection in goingToRoom.connectionsLeft)
                {
                    if (!connection.open) continue;
                    goingToRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(0, 1));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(0, 1));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates);
                    goingToRoom.connectionLayer.EraseCell(connection.coordinates);
                }
                break;

            case Room.ConnectDirection.Up:
                foreach (Room.Connection connection in comingFromRoom.connectionsUp)
                {
                    if (!connection.open) continue;
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(1, 0));
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(1, 0));
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates);
                    comingFromRoom.connectionLayer.EraseCell(connection.coordinates);
                }
                foreach (Room.Connection connection in goingToRoom.connectionsDown)
                {
                    if (!connection.open) continue;
                    goingToRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(1, 0));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(1, 0));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates);
                    goingToRoom.connectionLayer.EraseCell(connection.coordinates);
                }
                break;

            case Room.ConnectDirection.Down:
                foreach (Room.Connection connection in comingFromRoom.connectionsDown)
                {
                    if (!connection.open) continue;
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(1, 0));
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(1, 0));
                    comingFromRoom.decorLayer.EraseCell(connection.coordinates);
                    comingFromRoom.connectionLayer.EraseCell(connection.coordinates);
                }
                foreach (Room.Connection connection in goingToRoom.connectionsUp)
                {
                    if (!connection.open) continue;
                    goingToRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(1, 0));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(1, 0));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates);
                    goingToRoom.connectionLayer.EraseCell(connection.coordinates);
                }
                break;
        }
    }
}
