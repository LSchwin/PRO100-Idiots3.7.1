using Godot;
using System;
using System.Collections.Generic;
using GDArray = Godot.Collections.Array;

public partial class GameCS : Node
{
    public static GameCS Instance { get; private set; }

    public GDArray deck = new GDArray();
    public GDArray hand = new GDArray();
    public float Notoriety { get; set { field = (value < 0.0f) ? 0.0f : value; } }
    public int Money { get; set { field = (value < 0) ? 0 : value; } }
    public int BaseHealth { get; set { field = (value < 0) ? 0 : value; } } = 5;
    public int MaxHealth { get; set { field = (value < 0) ? 0 : value; } } = 5;
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
    } = 5;
    public int BaseAtk { get; set { field = (value < 0) ? 0 : value; } }
    public int Atk { get; set { field = (value < 0) ? 0 : value; } }
    public float BaseAtkCooldown { get; set { field = (value < 0) ? 0 : value; } } = 0.3f;
    public float AtkCooldown { get; set { field = (value < 0) ? 0 : value; } } = 0.3f;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        var passive = GD.Load<CardResource>("uid://d1hdj7c0phhqv");
        var active = GD.Load<CardResource>("uid://cibfkthvglfax");

        AddToDeck(active);
        MoveToHand(active);
    }

    public GDArray GetDeck()
    {
        return deck;
    }
    public GDArray GetHand()
    {
        return hand;
    }

    public static bool SwapCard(CardResource idle, CardResource active)
    {
        bool removedFromDeck = false;
        bool removedFromActive = false;

        if (Instance.deck.Contains(idle) && Instance.hand.Contains(active))
        {
            removedFromDeck = Instance.deck.Remove(idle);
            Instance.deck.Add(active);
            removedFromActive = Instance.hand.Remove(active);
            Instance.hand.Add(idle);
        }
        return removedFromActive && removedFromDeck;
    }

    public static void AddToDeck(CardResource card)
    {
        Instance.deck.Add(card);
        GD.Print($"Added {card.Name} to deck");
        CalculateCardBonuses();
    }
    public static bool RemoveFromDeck(CardResource card)
    {
        bool success = Instance.deck.Remove(card);
        CalculateCardBonuses();
        GD.Print(success ? $"Removed {card.Name} from deck" : $"{card.Name} does not exist in deck");
        return success;
    }

    public bool MoveToHand(CardResource card)
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
        if (!Instance.hand.Contains(card)) return false;
        if (card.OnCooldown)
        {
            GD.Print($"{card.Name} is on cooldown, cannot remove from hand");
            return false;
        }

        Instance.deck.Add(card);
        bool success = Instance.hand.Remove(card);
        GD.Print(success ? $"Removed {card.Name} from hand" : $"{card.Name} does not exist in hand");
        return success;
    }

    public static bool RotateFromHand(CardResource card)
    {
        if (Instance.hand.Contains(card)) return RemoveFromHand(card);
        else if (Instance.deck.Contains(card)) return Instance.MoveToHand(card);
        return false;
    }

    public static void CalculateCardBonuses()
    {
        // "reset" values
        int health = Instance.BaseHealth;
        int atk = Instance.BaseAtk;

        // flat bonuses first, then multipliers
        foreach (CardResource card in Instance.deck)
        {
            var dict = card.GetPassives();
            if (dict.ContainsKey("attack")) atk += (int)dict["attack"];
            if (dict.ContainsKey("health")) health += (int)dict["health"];
        }
        foreach (CardResource card in Instance.deck)
        {
            var dict = card.GetPassives();
            if (dict.ContainsKey("attackMult")) atk = (int)(atk * (float)dict["attackMult"]);
            if (dict.ContainsKey("healthMult")) health = (int)(health * (float)dict["healthMult"]);
        }

        if (health > Instance.MaxHealth)
        {
            int boost = health - Instance.MaxHealth;
            Instance.MaxHealth = health;
            Instance.Health += boost;
        }
        else
        {
            int current = Instance.Health;
            Instance.MaxHealth = health;
            // if your max health is reduced below your current health this will damage you
            // otherwise your health should remain the same
            Instance.Health = current;
        }
        Instance.Atk = atk;

        GD.Print($"attack:{Instance.Atk}\nhealth:{Instance.Health}/{Instance.MaxHealth}");
    }
}