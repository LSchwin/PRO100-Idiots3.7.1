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

    public bool AutoPickup { get; set; }
    public bool Interactable { get; set; }

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
