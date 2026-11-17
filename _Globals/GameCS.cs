using Godot;
using System;

public partial class GameCS : Node
{
	public static GameCS Instance { get; private set; }

	public Godot.Collections.Array<CardResource> deck;
	public Godot.Collections.Array<CardResource> activeCards;
    public float Notoriety { get; set { field = (value < 0.0f) ? 0.0f : value; } }
    public int Money { get; set { field = (value < 0) ? 0 : value; } }
    public int BaseHealth { get; set { field = (value < 0) ? 0 : value; } }
	public int MaxHealth { get; set { field = (value < 0) ? 0 : value; } }
    public int Health { 
		get; 
		set { 
			field = (value > MaxHealth) 
				? MaxHealth 
				: (value < 0) 
					? 0 
					: value; 
		} 
	}
    public int BaseAtk { get; set { field = (value < 0) ? 0 : value; } }
    public int Atk { get; set { field = (value < 0) ? 0 : value; } }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Instance = this;
	}

	public bool SwapCard(CardResource idle, CardResource active)
	{
		bool removedFromDeck = false;
		bool removedFromActive = false;

        if (deck.Contains(idle) && activeCards.Contains(active))
		{
            removedFromDeck = deck.Remove(idle);
			deck.Add(active);
			removedFromActive = activeCards.Remove(active);
			activeCards.Add(idle);
		}
		return removedFromActive && removedFromDeck;
	}

	public void AddToDeck(CardResource card)
	{
		deck.Add(card);
	}
	public bool RemoveFromDeck(CardResource card)
	{
		return deck.Remove(card);
	}
}
