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
        // the actual code begins here


        foreach (var room in roomOrder)
        {
            Room oldRoom = allUsedRooms.ElementAt(allUsedRooms.Count - 1);

            Connection oldConnection = default;
            // maybe choose connection from previous room instead

            //Enum.GetValues<ConnectDirection>().Shuffle(); for loop this _ times, check if left or right + GetAvailableConnections() is not null


            foreach (var list in oldRoom.allConnections)
            {
                foreach (var connection in list)
                {
                    if (connection.open)
                    {
                        // choose random room 

                        // check if they have an available connection for 'connection' (may need to Instantiate then AddChild first)
                        // switch(connection.direction) 

                        //switch (connection.direction)
                        //{
                        //    case ConnectDirection.Left:
                        //        directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsLeft);
                        //        break;
                        //    case ConnectDirection.Right:
                        //        directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsRight);
                        //        break;
                        //    case ConnectDirection.Up:
                        //        directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsUp);
                        //        break;
                        //    case ConnectDirection.Down:
                        //        directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsDown);
                        //        break;
                        //}

                        //null check for directionConnections

                    }
                }
            }
            // 


            bool roomCreatedSuccessfully = false;
            do
            {
                bool roomChosenSuccesfully = false;
                do
                {
                    var roomArray = AvailableRooms[room];
                    var chosenRoom = roomArray.ElementAt((int)GD.Randi() % roomArray.Count);

                    Room createdRoom = chosenRoom.Instantiate<Room>();

                    AddChild(createdRoom);
                    allUsedRooms.Add(createdRoom);
                    roomChosenSuccesfully = true;

                    bool connectionChosenSuccesfully = false;
                    do
                    { 
                        var randomDirection = Enum.GetValues<ConnectDirection>().Shuffle();
                        if (randomDirection.First() == ConnectDirection.Up || randomDirection.First() == ConnectDirection.Down)
                        {
                            randomDirection = Enum.GetValues<ConnectDirection>().Shuffle();
                        }

                        foreach (var direction in randomDirection)
                        {
                            List<Connection> directionConnections = default;
                            switch (direction)
                            {
                                case ConnectDirection.Left:
                                    directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsLeft);
                                    break;
                                case ConnectDirection.Right:
                                    directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsRight);
                                    break;
                                case ConnectDirection.Up:
                                    directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsUp);
                                    break;
                                case ConnectDirection.Down:
                                    directionConnections = createdRoom.GetAvailableConnections(createdRoom.connectionsDown);
                                    break;
                            }
                        
                            directionConnections.Shuffle();
                            foreach(var connection in directionConnections)
                            {
                                if (AttemptConnectRooms(allUsedRooms.ElementAt(allUsedRooms.Count - 2), oldConnection, createdRoom, connection))
                                {
                                    break;
                                }
                            }
                        
                        }

                    } while (connectionChosenSuccesfully == false);

                } while (roomChosenSuccesfully == false);


            } while (roomCreatedSuccessfully == false);
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
