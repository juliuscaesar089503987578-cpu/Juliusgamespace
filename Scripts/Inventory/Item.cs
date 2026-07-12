using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents a single item in the game.
/// </summary>
[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable,
        Material,
        Tool,
        Ammo,
        Miscellaneous
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public int id;
    public string name;
    public string description;
    public ItemType itemType;
    public Rarity rarity;
    public float weight;
    public int maxStack;
    public int currentStack = 1;
    public float durability = 100f;
    public float maxDurability = 100f;
    public float value; // Trade value
    public Sprite icon;

    // Special properties
    public Dictionary<string, float> properties = new Dictionary<string, float>();

    public Item()
    {
    }

    public Item(Item original)
    {
        id = original.id;
        name = original.name;
        description = original.description;
        itemType = original.itemType;
        rarity = original.rarity;
        weight = original.weight;
        maxStack = original.maxStack;
        currentStack = 1;
        durability = original.durability;
        maxDurability = original.maxDurability;
        value = original.value;
        icon = original.icon;
        properties = new Dictionary<string, float>(original.properties);
    }

    /// <summary>
    /// Get total weight of stacked items
    /// </summary>
    public float GetTotalWeight() => weight * currentStack;

    /// <summary>
    /// Check if item can stack
    /// </summary>
    public bool CanStack() => maxStack > 1;

    /// <summary>
    /// Degrade durability
    /// </summary>
    public void DegraeDurability(float amount)
    {
        durability -= amount;
        durability = Mathf.Max(durability, 0);
    }

    /// <summary>
    /// Get durability percentage
    /// </summary>
    public float GetDurabilityPercent() => durability / maxDurability;

    /// <summary>
    /// Check if item is broken
    /// </summary>
    public bool IsBroken() => durability <= 0;

    /// <summary>
    /// Get item color based on rarity
    /// </summary>
    public Color GetRarityColor() => rarity switch
    {
        Rarity.Common => Color.white,
        Rarity.Uncommon => Color.green,
        Rarity.Rare => Color.cyan,
        Rarity.Epic => new Color(0.6f, 0.2f, 1f), // Purple
        Rarity.Legendary => new Color(1f, 0.84f, 0f), // Gold
        _ => Color.white
    };
}

/// <summary>
/// Database of all item definitions in the game.
/// </summary>
public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    private static ItemDatabase instance;

    public static ItemDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemDatabase>();
                if (instance == null)
                {
                    GameObject go = new GameObject("ItemDatabase");
                    instance = go.AddComponent<ItemDatabase>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            InitializeDatabase();
        }
    }

    /// <summary>
    /// Initialize all items in the database
    /// </summary>
    private void InitializeDatabase()
    {
        if (items.Count == 0)
        {
            CreateDefaultItems();
        }
    }

    /// <summary>
    /// Create default items for testing
    /// </summary>
    private void CreateDefaultItems()
    {
        // Weapons
        items.Add(new Item
        {
            id = 1,
            name = "Iron Sword",
            description = "A basic iron sword",
            itemType = Item.ItemType.Weapon,
            rarity = Item.Rarity.Common,
            weight = 2f,
            maxStack = 1,
            durability = 100f,
            maxDurability = 100f,
            value = 50f,
            properties = new Dictionary<string, float> { { "damage", 15f }, { "attack_speed", 1.2f } }
        });

        // Consumables
        items.Add(new Item
        {
            id = 2,
            name = "Canned Food",
            description = "Nutritious canned food",
            itemType = Item.ItemType.Consumable,
            rarity = Item.Rarity.Common,
            weight = 0.3f,
            maxStack = 99,
            durability = 100f,
            maxDurability = 100f,
            value = 10f,
            properties = new Dictionary<string, float> { { "hunger_restore", 30f } }
        });

        items.Add(new Item
        {
            id = 3,
            name = "Water Bottle",
            description = "Clean drinking water",
            itemType = Item.ItemType.Consumable,
            rarity = Item.Rarity.Common,
            weight = 0.5f,
            maxStack = 99,
            durability = 100f,
            maxDurability = 100f,
            value = 5f,
            properties = new Dictionary<string, float> { { "thirst_restore", 40f } }
        });

        // Materials
        items.Add(new Item
        {
            id = 4,
            name = "Wood Plank",
            description = "Basic building material",
            itemType = Item.ItemType.Material,
            rarity = Item.Rarity.Common,
            weight = 1f,
            maxStack = 99,
            durability = 100f,
            maxDurability = 100f,
            value = 5f
        });

        items.Add(new Item
        {
            id = 5,
            name = "Iron Ore",
            description = "Raw iron ore for crafting",
            itemType = Item.ItemType.Material,
            rarity = Item.Rarity.Common,
            weight = 1.5f,
            maxStack = 99,
            durability = 100f,
            maxDurability = 100f,
            value = 15f
        });

        // Medical
        items.Add(new Item
        {
            id = 6,
            name = "Bandage",
            description = "Stops bleeding",
            itemType = Item.ItemType.Consumable,
            rarity = Item.Rarity.Common,
            weight = 0.1f,
            maxStack = 99,
            durability = 100f,
            maxDurability = 100f,
            value = 10f,
            properties = new Dictionary<string, float> { { "healing", 20f } }
        });
    }

    /// <summary>
    /// Get item by ID
    /// </summary>
    public Item GetItemByID(int id)
    {
        Item foundItem = items.Find(item => item.id == id);
        return foundItem != null ? new Item(foundItem) : null;
    }

    /// <summary>
    /// Get item by name
    /// </summary>
    public Item GetItemByName(string name)
    {
        Item foundItem = items.Find(item => item.name == name);
        return foundItem != null ? new Item(foundItem) : null;
    }

    /// <summary>
    /// Get all items
    /// </summary>
    public List<Item> GetAllItems() => new List<Item>(items);
}
