using Godot;
using System;

public partial class Level : Node2D
{
    public int enemyScore { get; set; }
    public int lootScore { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		var room1 = GetNode<Room>("Room1");
		room1.comingFrom = Room.ConnectDirection.Left;
		room1.goingTo = Room.ConnectDirection.Right;
		var room2 = GetNode<Room>("Room2");
        room1.comingFrom = Room.ConnectDirection.Left;
        room1.goingTo = Room.ConnectDirection.Right;



        DeleteConnection(room1, room2);
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
		// in the comment is just some random test code
		//var coords = comingFromRoom.connectionLayer.GetUsedCellsById(0, new Vector2I(0, 0));
		//foreach (var coord in coords)
		//{
		//    comingFromRoom.connectionLayer.EraseCell(coord);
		//	  comingFromRoom.decorLayer.EraseCell(coord);
		//}
		//comingFrom.connectionLayer.SetCell();
		switch(comingFromRoom.goingTo)
		{
			case Room.ConnectDirection.Left:

				break;

            case Room.ConnectDirection.Right:
				foreach(Room.Connection connection in comingFromRoom.connectionsRight)
				{
					if (!connection.open) continue;
					GD.Print("te");
					comingFromRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(0, 1));
					comingFromRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(0, 1));
					comingFromRoom.decorLayer.EraseCell(connection.coordinates);
					comingFromRoom.connectionLayer.EraseCell(connection.coordinates);
				}
                foreach (Room.Connection connection in goingToRoom.connectionsLeft)
                {
                    if (!connection.open) continue;
                    GD.Print("teeeeee");
                    goingToRoom.decorLayer.EraseCell(connection.coordinates - new Vector2I(0, 1));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates + new Vector2I(0, 1));
                    goingToRoom.decorLayer.EraseCell(connection.coordinates);
                    goingToRoom.connectionLayer.EraseCell(connection.coordinates);
                }


                break;

            case Room.ConnectDirection.Up:

                break;

            case Room.ConnectDirection.Down:

                break;
        }
    }
}
