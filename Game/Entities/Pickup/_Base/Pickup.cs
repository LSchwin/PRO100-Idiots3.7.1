using Godot;
using System;

public partial class Pickup : WorldObject
{

    // TODO: Implement item given by the pickup

    //private Item item;
    //
    //public Item GetItem()
    //{
    //  return item;
    //}
    //public void SetItem(Item item)
    //{
    //  this.item = item;
    //}

    [Export]
    public bool AutoPickup { get; set; }

    [Export]
    public bool Interactable { get; set; }

    [Export]
    public int LootScore { 
        get; 
        set
        {
            if (value < 0) field = 0;
            else field = value;
        }
    }
    public void Interact()
    {
        Console.WriteLine("Interact called!");
    }
}
