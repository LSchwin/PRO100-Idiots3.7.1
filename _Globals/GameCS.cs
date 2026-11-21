using Godot;
using System;
using System.Collections.Generic;
using GDArray = Godot.Collections.Array;

[GlobalClass]
public partial class GameCS : Node
{
    public static GameCS Instance { get; private set; }

    public static GDArray deck = new GDArray();
    public static GDArray hand = new GDArray();
    public static float Notoriety { get; set { field = (value < 0.0f) ? 0.0f : value; } }
    public static int Money { get; set { field = (value < 0) ? 0 : value; } }
    public static int BaseHealth { get; set { field = (value < 0) ? 0 : value; } }
    public static int MaxHealth { get; set { field = (value < 0) ? 0 : value; } }
    public static int Health
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
    public static int BaseAtk { get; set { field = (value < 0) ? 0 : value; } }
    public static int Atk { get; set { field = (value < 0) ? 0 : value; } }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        var passive = GD.Load<CardResource>("uid://d1hdj7c0phhqv");
        var active = GD.Load<CardResource>("uid://cibfkthvglfax");

        AddToDeck(active);
        MoveToHand(active);
    }

    public bool SwapCard(CardResource idle, CardResource active)
    {
        bool removedFromDeck = false;
        bool removedFromActive = false;

        if (deck.Contains(idle) && hand.Contains(active))
        {
            removedFromDeck = deck.Remove(idle);
            deck.Add(active);
            removedFromActive = hand.Remove(active);
            hand.Add(idle);
        }
        return removedFromActive && removedFromDeck;
    }

    public void AddToDeck(CardResource card)
    {
        deck.Add(card);
        GD.Print($"Added {card.Name} to deck");
        CalculateCardBonuses();
    }
    public bool RemoveFromDeck(CardResource card)
    {
        bool success = deck.Remove(card);
        CalculateCardBonuses();
        GD.Print(success ? $"Removed {card.Name} from deck" : $"{card.Name} does not exist in deck");
        return success;
    }

    public static bool MoveToHand(CardResource card)
    {
        // can't move a card to your hand if it doesn't exist in your deck
        if (!deck.Contains(card)) return false;

        hand.Add(card);
        bool success = deck.Remove(card);
        GD.Print($"Added {card.Name} to hand from deck");
        return success;
    }

    public static bool RemoveFromHand(CardResource card)
    {
        // can't remove a card from your hand if it's not there
        if (!hand.Contains(card)) return false;
        if (card.OnCooldown)
        {
            GD.Print($"{card.Name} is on cooldown, cannot remove from hand");
            return false;
        }

        deck.Add(card);
        bool success = hand.Remove(card);
        GD.Print(success ? $"Removed {card.Name} from hand" : $"{card.Name} does not exist in hand");
        return success;
    }

    public static bool RotateFromHand(CardResource card)
    {
        if (hand.Contains(card)) return RemoveFromHand(card);
        else if (deck.Contains(card)) return MoveToHand(card);
        return false;
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