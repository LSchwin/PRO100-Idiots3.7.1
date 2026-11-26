using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //var room5 = GetNode<Room>("Room5");
        ////room5.comingFrom = Room.ConnectDirection.Up;
        //room5.goingTo = Room.ConnectDirection.Down;
        //
        //var room4 = GetNode<Room>("Room4");
        //room4.comingFrom = Room.ConnectDirection.Up;
        //room4.goingTo = Room.ConnectDirection.Right;
        //
        //DeleteConnection(room5, room4); 
        //
        //var room1 = GetNode<Room>("Room1");
		//room1.comingFrom = Room.ConnectDirection.Left;
		//room1.goingTo = Room.ConnectDirection.Right;
        //
        //DeleteConnection(room4, room1); 
        //
		//var room2 = GetNode<Room>("Room2");
        //room2.comingFrom = Room.ConnectDirection.Left;
        //room2.goingTo = Room.ConnectDirection.Down;
        //
        //DeleteConnection(room1, room2);
        //
        //var room3 = GetNode<Room>("Room3");
        //room3.comingFrom = Room.ConnectDirection.Up;
        //room3.goingTo = Room.ConnectDirection.Down;
        //
        //DeleteConnection(room2, room3);


        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Generate()
	{
        GenerateMainBranch();
    }

    public void GenerateMainBranch()
    {
        foreach (var room in roomOrder)
        {
            bool roomChosenSuccesfully = false;
            do
            {
                Room oldRoom = allUsedRooms.ElementAt(allUsedRooms.Count - 1);
                var prevRoomConnectionList = GetPreviousRoomConnections(oldRoom);

                ShuffleRoomListWithHorizontalWeight(ref prevRoomConnectionList);
                foreach (var list in prevRoomConnectionList)
                {
                    var test = list.First().direction;
                    // set oldRoom.goingTo with a switch
                    switch (test)
                    {
                        case ConnectDirection.Left:
                            oldRoom.goingTo = ConnectDirection.Right;
                            break;
                        case ConnectDirection.Right:
                            oldRoom.goingTo = ConnectDirection.Left;
                            break;
                        case ConnectDirection.Up:
                            oldRoom.goingTo = ConnectDirection.Down;
                            break;
                        case ConnectDirection.Down:
                            oldRoom.goingTo = ConnectDirection.Up;
                            break;
                    }

                    foreach (var oldRoomConnection in list)
                    {
                        if (oldRoomConnection.open)
                        {
                            // choose random room 
                            var roomArray = AvailableRooms[room];
                            var chosenRoom = roomArray.ElementAt((int)GD.Randi() % roomArray.Count);

                            Room createdRoom = chosenRoom.Instantiate<Room>();
                            AddChild(createdRoom);
                            allUsedRooms.Add(createdRoom);

                            List<Connection> newRoomConnections = default;
                            switch (oldRoom.goingTo)
                            {
                                case ConnectDirection.Left:
                                    newRoomConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsLeft);
                                    break;
                                case ConnectDirection.Right:
                                    newRoomConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsRight);
                                    break;
                                case ConnectDirection.Up:
                                    newRoomConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsUp);
                                    break;
                                case ConnectDirection.Down:
                                    newRoomConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsDown);
                                    break;
                            }

                            newRoomConnections.Shuffle();
                            foreach (var connection in newRoomConnections)
                            {
                                if (AttemptConnectRooms(allUsedRooms.ElementAt(allUsedRooms.Count - 2), oldRoomConnection, createdRoom, connection))
                                {
                                    roomChosenSuccesfully = true;
                                    break;
                                }
                            }

                            if (!roomChosenSuccesfully)
                            {
                                allUsedRooms.Remove(createdRoom);
                                createdRoom.QueueFree();
                            }
                        }
                        if (roomChosenSuccesfully) { break; }
                    }
                    if (roomChosenSuccesfully) { break; }
                }

                if (roomChosenSuccesfully) 
                { 
                    break;
                }
                else
                {
                    var previousRoom = allUsedRooms.Last();
                    allUsedRooms.Remove(previousRoom);
                    previousRoom.QueueFree();
                }
            } while (roomChosenSuccesfully == false);
        }
    }

    public List<List<Connection>> GetPreviousRoomConnections(Room oldRoom)
    {
        List<List<Connection>> returnList = new List<List<Connection>>();

        switch (oldRoom.connectionDirection)
        {
            case AvailableConnectionDirection.HorizontalOnly:
                returnList.Add(oldRoom.connectionsLeft);
                returnList.Add(oldRoom.connectionsRight);
                break;
            case AvailableConnectionDirection.VerticalOnly:
                returnList.Add(oldRoom.connectionsUp);
                returnList.Add(oldRoom.connectionsDown);
                break;
            case AvailableConnectionDirection.All:
                returnList.Add(oldRoom.connectionsUp);
                returnList.Add(oldRoom.connectionsDown);
                returnList.Add(oldRoom.connectionsLeft);
                returnList.Add(oldRoom.connectionsRight);
                break;
            case AvailableConnectionDirection.LeftDown:
                returnList.Add(oldRoom.connectionsLeft);
                returnList.Add(oldRoom.connectionsDown);
                break;
            case AvailableConnectionDirection.RightDown:
                returnList.Add(oldRoom.connectionsDown);
                returnList.Add(oldRoom.connectionsRight);
                break;
            case AvailableConnectionDirection.UpRight:
                returnList.Add(oldRoom.connectionsRight);
                returnList.Add(oldRoom.connectionsUp);
                break;
            case AvailableConnectionDirection.UpLeft:
                returnList.Add(oldRoom.connectionsUp);
                returnList.Add(oldRoom.connectionsLeft);
                break;
            case AvailableConnectionDirection.NoLeft:
                returnList.Add(oldRoom.connectionsUp);
                returnList.Add(oldRoom.connectionsDown);
                returnList.Add(oldRoom.connectionsRight);
                break;
            case AvailableConnectionDirection.NoRight:
                returnList.Add(oldRoom.connectionsUp);
                returnList.Add(oldRoom.connectionsDown);
                returnList.Add(oldRoom.connectionsLeft);
                break;
            case AvailableConnectionDirection.NoUp:
                returnList.Add(oldRoom.connectionsRight);
                returnList.Add(oldRoom.connectionsDown);
                returnList.Add(oldRoom.connectionsLeft);
                break;
            case AvailableConnectionDirection.NoDown:
                returnList.Add(oldRoom.connectionsRight);
                returnList.Add(oldRoom.connectionsDown);
                returnList.Add(oldRoom.connectionsLeft);
                break;
            default:
                GD.PushError("AvailableConnectionDirection CANNOT be NULL");
                break;
        }

        return returnList;
    }

    // shuffles the List of connection lists with more weight for horizontal stuff
    public void ShuffleRoomListWithHorizontalWeight(ref List<List<Connection>> connectionList)
    {
        for(int i = 0; i < 2; ++i)
        {
            connectionList.Shuffle();
            if ((connectionList.First().First().direction == Room.ConnectDirection.Left) || 
                (connectionList.First().First().direction == Room.ConnectDirection.Right)) { break; }
        }
    }

    public void GenerateSideBranches()
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

    // return true if the room has been placed and is not colliding with other rooms, false if no
    private bool AttemptConnectRooms(Room oldRoom, Connection oldRoomConnection, Room newRoom, Connection newRoomConnection)
    {
        return false;
    }

    //depreciated method
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

    //depreciated method (meaning im gonna replace it with something better)
    private void EraseConnection(Room room, List<Connection> connections, Vector2I tileCoordinates)
    {
        foreach (Room.Connection connection in connections)
        {
            if (!connection.open) continue;
            room.decorLayer.EraseCell(connection.coordinatesLocal - tileCoordinates);
            room.decorLayer.EraseCell(connection.coordinatesLocal + tileCoordinates);
            room.decorLayer.EraseCell(connection.coordinatesLocal);
            room.connectionLayer.EraseCell(connection.coordinatesLocal);

            //var num = room.connectionLayer.ToGlobal(connection.coordinates);
            //var num2 = room.connectionLayer.ToLocal(num);
            //
            //GD.Print(connection.coordinates);
            //GD.Print(num);
            //GD.Print(num2);
        }
    }

    // TODO: finish this
    private void DeleteAllOpenConnections()
    {
        foreach(var room in allUsedRooms)
        {
            foreach (var list in room.allConnections)
            {
                foreach (var connection in list)
                {
                    //if (connection.open) //delete connection method that figures out which connection it is
                }
            }
        }
    }


}
