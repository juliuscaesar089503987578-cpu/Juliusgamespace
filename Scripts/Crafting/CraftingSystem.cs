using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the crafting system and crafting queue.
/// </summary>
public class CraftingSystem : MonoBehaviour
{
    private Queue<CraftingJob> craftingQueue = new Queue<CraftingJob>();
    private CraftingJob currentJob = null;
    private float craftingProgress = 0f;
    private bool isCrafting = false;

    public bool IsCrafting => isCrafting;
    public float CraftingProgress => craftingProgress;
    public CraftingJob CurrentJob => currentJob;

    [System.Serializable]
    public class CraftingJob
    {
        public Recipe recipe;
        public int quantity = 1;
        public float timeElapsed = 0f;

        public float GetTotalTime() => recipe.craftingTime * quantity;
        public float GetProgress() => Mathf.Clamp01(timeElapsed / GetTotalTime());
    }

    private void Update()
    {
        if (isCrafting && currentJob != null)
        {
            UpdateCrafting();
        }
    }

    /// <summary>
    /// Queue a recipe to craft
    /// </summary>
    public bool QueueRecipe(Recipe recipe, int quantity = 1)
    {
        if (recipe == null)
        {
            Debug.LogError("Recipe is null");
            return false;
        }

        Player player = GameManager.Instance.GetCurrentPlayer();
        if (player == null) return false;

        // Check if recipe can be crafted
        if (!recipe.CanCraft(player.Inventory, player.SkillTree))
        {
            Debug.LogWarning($"Cannot craft {recipe.name} - missing ingredients or skills");
            return false;
        }

        // Create crafting job
        CraftingJob job = new CraftingJob
        {
            recipe = recipe,
            quantity = quantity
        };

        craftingQueue.Enqueue(job);
        GameManager.Instance.EventManager.TriggerEvent("RecipeQueued", recipe.name);

        // Start crafting if not already
        if (!isCrafting)
        {
            StartCrafting();
        }

        return true;
    }

    /// <summary>
    /// Start crafting next recipe in queue
    /// </summary>
    private void StartCrafting()
    {
        if (craftingQueue.Count == 0)
        {
            isCrafting = false;
            return;
        }

        currentJob = craftingQueue.Dequeue();
        isCrafting = true;
        craftingProgress = 0f;

        GameManager.Instance.EventManager.TriggerEvent("CraftingStarted", currentJob.recipe.name);
    }

    /// <summary>
    /// Update crafting progress
    /// </summary>
    private void UpdateCrafting()
    {
        if (currentJob == null) return;

        Player player = GameManager.Instance.GetCurrentPlayer();
        if (player == null) return;

        // Get adjusted craft time based on intelligence
        float adjustedTime = currentJob.recipe.GetAdjustedCraftTime(player.CharacterData.attributes.intelligence);
        currentJob.timeElapsed += Time.deltaTime;
        craftingProgress = currentJob.GetProgress();

        GameManager.Instance.EventManager.TriggerEvent("CraftingProgress", craftingProgress);

        // Check if crafting is complete
        if (currentJob.timeElapsed >= currentJob.GetTotalTime())
        {
            CompleteCrafting();
        }
    }

    /// <summary>
    /// Complete current crafting job
    /// </summary>
    private void CompleteCrafting()
    {
        if (currentJob == null) return;

        Player player = GameManager.Instance.GetCurrentPlayer();
        if (player == null) return;

        Recipe recipe = currentJob.recipe;
        int quantity = currentJob.quantity;

        // Remove ingredients from inventory
        foreach (var ingredient in recipe.ingredients)
        {
            int needed = ingredient.quantity * quantity;
            List<Item> matchingItems = player.Inventory.FindItemsByType(Item.ItemType.Material);
            
            foreach (var item in matchingItems)
            {
                if (item.name == ingredient.itemName && needed > 0)
                {
                    int removeAmount = Mathf.Min(needed, item.currentStack);
                    needed -= removeAmount;
                }
            }
        }

        // Add crafted item to inventory
        Item resultItem = new Item(ItemDatabase.Instance.GetItemByID(recipe.id))
        {
            currentStack = recipe.resultQuantity * quantity
        };
        player.Inventory.AddItem(resultItem, out var remainder);

        // Gain experience and skill
        player.Stats.GainExperience(recipe.id * 10);
        if (!string.IsNullOrEmpty(recipe.requiredSkill))
        {
            player.SkillTree.IncreaseSkill(recipe.requiredSkill, 1);
        }

        GameManager.Instance.EventManager.TriggerEvent("CraftingCompleted", recipe.name);

        // Move to next job
        if (craftingQueue.Count > 0)
        {
            StartCrafting();
        }
        else
        {
            isCrafting = false;
            currentJob = null;
        }
    }

    /// <summary>
    /// Cancel current crafting job
    /// </summary>
    public void CancelCrafting()
    {
        if (currentJob != null)
        {
            GameManager.Instance.EventManager.TriggerEvent("CraftingCancelled", currentJob.recipe.name);
            currentJob = null;
            isCrafting = false;
            
            if (craftingQueue.Count > 0)
            {
                StartCrafting();
            }
        }
    }

    /// <summary>
    /// Get number of jobs in queue
    /// </summary>
    public int GetQueueSize() => craftingQueue.Count + (isCrafting ? 1 : 0);
}
