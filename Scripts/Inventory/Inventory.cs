using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Grid-based inventory system with weight management.
/// </summary>
public class Inventory
{
    public const int GRID_WIDTH = 10;
    public const int GRID_HEIGHT = 10;
    public const int TOTAL_SLOTS = GRID_WIDTH * GRID_HEIGHT;

    private Item[,] grid;
    private List<Item> equipment; // Head, chest, legs, feet, hands, back
    private List<Item> quickSlots; // 8 quick access slots
    private float maxCarryWeight;
    private float currentWeight = 0f;

    public float CurrentWeight => currentWeight;
    public float MaxCarryWeight => maxCarryWeight;
    public float RemainingCapacity => maxCarryWeight - currentWeight;
    public bool IsFull => currentWeight >= maxCarryWeight;

    public Inventory(float maxWeight = 100f)
    {
        maxCarryWeight = maxWeight;
        grid = new Item[GRID_HEIGHT, GRID_WIDTH];
        equipment = new List<Item>(7); // 7 equipment slots
        quickSlots = new List<Item>(8); // 8 quick slots
    }

    /// <summary>
    /// Add item to inventory
    /// </summary>
    public bool AddItem(Item item, out Item remainder)
    {
        remainder = null;

        if (item == null) return false;
        if (currentWeight + item.GetTotalWeight() > maxCarryWeight)
        {
            remainder = item;
            return false;
        }

        // Try to stack if possible
        if (item.CanStack())
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    if (grid[y, x] != null && grid[y, x].id == item.id && grid[y, x].currentStack < grid[y, x].maxStack)
                    {
                        int spaceInStack = grid[y, x].maxStack - grid[y, x].currentStack;
                        int itemsToAdd = Mathf.Min(spaceInStack, item.currentStack);

                        grid[y, x].currentStack += itemsToAdd;
                        currentWeight += (itemsToAdd * item.weight);

                        if (itemsToAdd < item.currentStack)
                        {
                            remainder = new Item(item);
                            remainder.currentStack -= itemsToAdd;
                            return true;
                        }

                        GameManager.Instance?.EventManager.TriggerEvent("ItemAdded", item.name);
                        return true;
                    }
                }
            }
        }

        // Find empty slot
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                if (grid[y, x] == null)
                {
                    grid[y, x] = new Item(item);
                    currentWeight += item.GetTotalWeight();
                    GameManager.Instance?.EventManager.TriggerEvent("ItemAdded", item.name);
                    return true;
                }
            }
        }

        remainder = item;
        return false;
    }

    /// <summary>
    /// Remove item from inventory
    /// </summary>
    public bool RemoveItem(int gridX, int gridY, int amount = 1)
    {
        if (gridX < 0 || gridX >= GRID_WIDTH || gridY < 0 || gridY >= GRID_HEIGHT)
            return false;

        if (grid[gridY, gridX] == null) return false;

        Item item = grid[gridY, gridX];
        int amountRemoved = Mathf.Min(amount, item.currentStack);

        item.currentStack -= amountRemoved;
        currentWeight -= (amountRemoved * item.weight);

        if (item.currentStack <= 0)
        {
            grid[gridY, gridX] = null;
        }

        GameManager.Instance?.EventManager.TriggerEvent("ItemRemoved", item.name);
        return true;
    }

    /// <summary>
    /// Move item within inventory
    /// </summary>
    public bool MoveItem(int fromX, int fromY, int toX, int toY)
    {
        if (!IsValidGridPosition(fromX, fromY) || !IsValidGridPosition(toX, toY))
            return false;

        if (grid[fromY, fromX] == null)
            return false;

        // Swap items
        Item temp = grid[toY, toX];
        grid[toY, toX] = grid[fromY, fromX];
        grid[fromY, fromX] = temp;

        return true;
    }

    /// <summary>
    /// Get item at grid position
    /// </summary>
    public Item GetItem(int gridX, int gridY)
    {
        if (!IsValidGridPosition(gridX, gridY)) return null;
        return grid[gridY, gridX];
    }

    /// <summary>
    /// Get all items in inventory
    /// </summary>
    public List<Item> GetAllItems()
    {
        List<Item> allItems = new List<Item>();
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                if (grid[y, x] != null)
                {
                    allItems.Add(grid[y, x]);
                }
            }
        }
        return allItems;
    }

    /// <summary>
    /// Equip item
    /// </summary>
    public bool EquipItem(Item item, int equipmentSlot)
    {
        if (equipmentSlot < 0 || equipmentSlot >= 7) return false;

        // Ensure list has enough capacity
        while (equipment.Count <= equipmentSlot)
        {
            equipment.Add(null);
        }

        equipment[equipmentSlot] = new Item(item);
        GameManager.Instance?.EventManager.TriggerEvent("ItemEquipped", item.name);
        return true;
    }

    /// <summary>
    /// Add to quick slot
    /// </summary>
    public bool AddToQuickSlot(Item item, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= 8) return false;

        while (quickSlots.Count <= slotIndex)
        {
            quickSlots.Add(null);
        }

        quickSlots[slotIndex] = new Item(item);
        return true;
    }

    /// <summary>
    /// Get equipment item
    /// </summary>
    public Item GetEquippedItem(int slot)
    {
        if (slot < 0 || slot >= equipment.Count) return null;
        return equipment[slot];
    }

    /// <summary>
    /// Get quick slot item
    /// </summary>
    public Item GetQuickSlotItem(int slot)
    {
        if (slot < 0 || slot >= quickSlots.Count) return null;
        return quickSlots[slot];
    }

    /// <summary>
    /// Find item by name
    /// </summary>
    public Item FindItem(string itemName)
    {
        return GetAllItems().FirstOrDefault(item => item.name == itemName);
    }

    /// <summary>
    /// Find items by type
    /// </summary>
    public List<Item> FindItemsByType(Item.ItemType type)
    {
        return GetAllItems().Where(item => item.itemType == type).ToList();
    }

    private bool IsValidGridPosition(int x, int y)
    {
        return x >= 0 && x < GRID_WIDTH && y >= 0 && y < GRID_HEIGHT;
    }

    /// <summary>
    /// Clear inventory
    /// </summary>
    public void Clear()
    {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                grid[y, x] = null;
            }
        }
        currentWeight = 0f;
        equipment.Clear();
        quickSlots.Clear();
    }

    /// <summary>
    /// Get inventory info
    /// </summary>
    public InventoryInfo GetInfo()
    {
        return new InventoryInfo
        {
            usedSlots = GetAllItems().Count,
            totalSlots = TOTAL_SLOTS,
            currentWeight = currentWeight,
            maxWeight = maxCarryWeight,
            itemCount = GetAllItems().Sum(item => item.currentStack)
        };
    }
}

[System.Serializable]
public class InventoryInfo
{
    public int usedSlots;
    public int totalSlots;
    public float currentWeight;
    public float maxWeight;
    public int itemCount;
}
