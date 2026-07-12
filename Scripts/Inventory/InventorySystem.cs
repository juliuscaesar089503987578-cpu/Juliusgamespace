using UnityEngine;
using System;

/// <summary>
/// Manages the inventory system as a core game system.
/// </summary>
public class InventorySystem : MonoBehaviour
{
    private Inventory playerInventory;

    public Inventory GetInventory() => playerInventory;

    /// <summary>
    /// Initialize inventory for the player
    /// </summary>
    public void InitializeInventory(Player player)
    {
        playerInventory = new Inventory(100f); // 100 kg capacity

        // Add starter items based on profession
        AddStarterItems(player.CharacterData.profession);

        Debug.Log("Inventory initialized");
        GameManager.Instance.EventManager.TriggerEvent("InventoryInitialized");
    }

    /// <summary>
    /// Add starter items based on profession
    /// </summary>
    private void AddStarterItems(int profession)
    {
        Item remainder;

        // All professions get basic supplies
        playerInventory.AddItem(ItemDatabase.Instance.GetItemByName("Canned Food"), out remainder);
        playerInventory.AddItem(ItemDatabase.Instance.GetItemByName("Water Bottle"), out remainder);
        playerInventory.AddItem(ItemDatabase.Instance.GetItemByName("Bandage"), out remainder);

        // Profession-specific items
        switch (profession)
        {
            case 0: // Medic
                playerInventory.AddItem(ItemDatabase.Instance.GetItemByName("Bandage"), out remainder);
                break;
            case 1: // Engineer
                playerInventory.AddItem(ItemDatabase.Instance.GetItemByName("Wood Plank"), out remainder);
                break;
            case 2: // Hunter
                playerInventory.AddItem(ItemDatabase.Instance.GetItemByName("Iron Ore"), out remainder);
                break;
            case 3: // Soldier
                playerInventory.AddItem(ItemDatabase.Instance.GetItemByName("Iron Sword"), out remainder);
                break;
        }
    }

    /// <summary>
    /// Add item to player inventory
    /// </summary>
    public bool AddItemToInventory(Item item)
    {
        if (playerInventory == null)
        {
            Debug.LogError("Inventory not initialized");
            return false;
        }

        Item remainder;
        return playerInventory.AddItem(item, out remainder);
    }

    /// <summary>
    /// Remove item from inventory
    /// </summary>
    public bool RemoveItemFromInventory(int gridX, int gridY, int amount = 1)
    {
        if (playerInventory == null) return false;
        return playerInventory.RemoveItem(gridX, gridY, amount);
    }

    /// <summary>
    /// Get inventory info for UI
    /// </summary>
    public InventoryInfo GetInventoryInfo()
    {
        return playerInventory?.GetInfo();
    }
}
