using Godot;
using System;
using System.Collections.Generic;
using GDArray = Godot.Collections.Array;

public partial class GameCS : Node
{
    public static GameCS Instance { get; private set; }

    public GDArray deck = new GDArray();
    public GDArray activeCards = new GDArray();
    public float Notoriety { get; set { field = (value < 0.0f) ? 0.0f : value; } }
    public int Money { get; set { field = (value < 0) ? 0 : value; } }
    public int BaseHealth { get; set { field = (value < 0) ? 0 : value; } }
    public int MaxHealth { get; set { field = (value < 0) ? 0 : value; } }
    public int Health
    {
        get;
        set
        {
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
        var card = GD.Load<CardResource>("uid://d1hdj7c0phhqv");

        AddToDeck(card);
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
        GD.Print($"Added card: {card.Name}");
        CalculateCardBonuses();
    }
    public bool RemoveFromDeck(CardResource card)
    {
        bool success = deck.Remove(card);
        CalculateCardBonuses();
        GD.Print(success ? $"Card removed: {card.Name}" : $"Could not remove card: {card.Name}");
        return success;
    }

    public void CalculateCardBonuses()
    {
        // "reset" values
        int health = BaseHealth;
        int atk = BaseAtk;

        // flat bonuses first, then multipliers
        foreach (CardResource card in deck)
        {
            var dict = card.GetPassives();
            if (dict.ContainsKey("attack")) atk += (int)dict["attack"];
            if (dict.ContainsKey("health")) health += (int)dict["health"];
        }
        foreach (CardResource card in deck)
        {
            var dict = card.GetPassives();
            if (dict.ContainsKey("attackMult")) atk = (int)(atk * (float)dict["attackMult"]);
            if (dict.ContainsKey("healthMult")) health = (int)(health * (float)dict["healthMult"]);
        }

        if (health > MaxHealth)
        {
            int boost = health - MaxHealth;
            MaxHealth = health;
            Health += boost;
        }
        else
        {
            int current = Health;
            MaxHealth = health;
            // if your max health is reduced below your current health this will damage you
            // otherwise your health should remain the same
            Health = current;
        }
        Atk = atk;

        GD.Print($"attack:{Atk}\nhealth:{Health}/{MaxHealth}");
    }
}