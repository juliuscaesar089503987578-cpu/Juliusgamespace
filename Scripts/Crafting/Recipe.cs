using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a crafting recipe.
/// </summary>
[System.Serializable]
public class Recipe
{
    public int id;
    public string name;
    public string description;
    public Item resultItem;
    public int resultQuantity = 1;
    public float craftingTime = 5f; // Seconds
    public List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
    public int requiredSkillLevel = 0;
    public string requiredSkill = "";
    public bool isUnlocked = false;

    [System.Serializable]
    public class RecipeIngredient
    {
        public string itemName;
        public int quantity;
    }

    /// <summary>
    /// Check if player can craft this recipe
    /// </summary>
    public bool CanCraft(Inventory inventory, SkillTree skillTree)
    {
        if (!isUnlocked) return false;
        if (!string.IsNullOrEmpty(requiredSkill) && skillTree.GetSkillLevel(requiredSkill) < requiredSkillLevel)
            return false;

        // Check if inventory has all ingredients
        foreach (var ingredient in ingredients)
        {
            int needed = ingredient.quantity;
            int found = 0;

            foreach (var item in inventory.GetAllItems())
            {
                if (item.name == ingredient.itemName)
                {
                    found += item.currentStack;
                }
            }

            if (found < needed) return false;
        }

        return true;
    }

    /// <summary>
    /// Get craft time adjusted by intelligence
    /// </summary>
    public float GetAdjustedCraftTime(int intelligence)
    {
        return craftingTime / (1f + (intelligence - 10) * 0.05f);
    }
}

/// <summary>
/// Database of all crafting recipes.
/// </summary>
public class RecipeDatabase : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    private static RecipeDatabase instance;

    public static RecipeDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RecipeDatabase>();
                if (instance == null)
                {
                    GameObject go = new GameObject("RecipeDatabase");
                    instance = go.AddComponent<RecipeDatabase>();
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
            InitializeRecipes();
        }
    }

    /// <summary>
    /// Initialize all recipes
    /// </summary>
    private void InitializeRecipes()
    {
        if (recipes.Count == 0)
        {
            CreateDefaultRecipes();
        }
    }

    /// <summary>
    /// Create default recipes for game
    /// </summary>
    private void CreateDefaultRecipes()
    {
        int recipeId = 1;

        // WEAPONS (10 recipes)
        recipes.Add(CreateWeaponRecipe(recipeId++, "Wooden Spear", 10f, new[] { ("Wood Plank", 5) }));
        recipes.Add(CreateWeaponRecipe(recipeId++, "Stone Axe", 15f, new[] { ("Wood Plank", 3), ("Stone", 2) }));
        recipes.Add(CreateWeaponRecipe(recipeId++, "Iron Sword", 20f, new[] { ("Iron Ore", 3), ("Wood Plank", 2) }, 5, "Crafting"));
        recipes.Add(CreateWeaponRecipe(recipeId++, "Steel Sword", 30f, new[] { ("Iron Ore", 5), ("Coal", 2) }, 15, "Crafting"));
        recipes.Add(CreateWeaponRecipe(recipeId++, "Longbow", 25f, new[] { ("Wood Plank", 4), ("Leather", 2) }, 10, "Crafting"));

        // ARMOR (10 recipes)
        recipes.Add(CreateArmorRecipe(recipeId++, "Leather Helmet", 15f, new[] { ("Leather", 2) }));
        recipes.Add(CreateArmorRecipe(recipeId++, "Leather Chest", 20f, new[] { ("Leather", 5) }, 5, "Crafting"));
        recipes.Add(CreateArmorRecipe(recipeId++, "Iron Helmet", 25f, new[] { ("Iron Ore", 3) }, 10, "Crafting"));
        recipes.Add(CreateArmorRecipe(recipeId++, "Iron Chest Plate", 35f, new[] { ("Iron Ore", 6) }, 15, "Crafting"));
        recipes.Add(CreateArmorRecipe(recipeId++, "Steel Armor Set", 50f, new[] { ("Iron Ore", 8), ("Coal", 3) }, 25, "Crafting"));

        // MEDICAL (15 recipes)
        recipes.Add(CreateMedicalRecipe(recipeId++, "Bandage", 2f, new[] { ("Cloth", 1) }));
        recipes.Add(CreateMedicalRecipe(recipeId++, "Splint", 5f, new[] { ("Wood Plank", 2), ("Cloth", 1) }, 5, "Medicine"));
        recipes.Add(CreateMedicalRecipe(recipeId++, "Antidote", 10f, new[] { ("Herb", 3), ("Water Bottle", 1) }, 10, "Medicine"));
        recipes.Add(CreateMedicalRecipe(recipeId++, "Antibiotic", 15f, new[] { ("Herb", 2), ("Chemical", 1) }, 15, "Medicine"));
        recipes.Add(CreateMedicalRecipe(recipeId++, "Pain Killer", 8f, new[] { ("Herb", 2) }, 10, "Medicine"));
        recipes.Add(CreateMedicalRecipe(recipeId++, "Vaccine", 20f, new[] { ("Chemical", 2), ("Herb", 1) }, 20, "Medicine"));
        recipes.Add(CreateMedicalRecipe(recipeId++, "Medical Kit", 30f, new[] { ("Bandage", 5), ("Antibiotic", 2), ("Splint", 2) }, 25, "Medicine"));

        // FOOD (15 recipes)
        recipes.Add(CreateFoodRecipe(recipeId++, "Cooked Meat", 5f, new[] { ("Raw Meat", 1) }));
        recipes.Add(CreateFoodRecipe(recipeId++, "Bread", 8f, new[] { ("Grain", 2) }));
        recipes.Add(CreateFoodRecipe(recipeId++, "Vegetable Stew", 10f, new[] { ("Vegetable", 3), ("Water Bottle", 1) }));
        recipes.Add(CreateFoodRecipe(recipeId++, "Meat Stew", 12f, new[] { ("Raw Meat", 2), ("Vegetable", 1), ("Water Bottle", 1) }, 5, "Cooking"));
        recipes.Add(CreateFoodRecipe(recipeId++, "Soup", 6f, new[] { ("Vegetable", 2), ("Water Bottle", 1) }));
        recipes.Add(CreateFoodRecipe(recipeId++, "Energy Bar", 8f, new[] { ("Grain", 1), ("Herb", 1) }, 5, "Cooking"));
        recipes.Add(CreateFoodRecipe(recipeId++, "Dried Fruit", 4f, new[] { ("Fruit", 3) }));

        // WATER (5 recipes)
        recipes.Add(CreateWaterRecipe(recipeId++, "Boiled Water", 3f, new[] { ("Water Bottle", 1) }));
        recipes.Add(CreateWaterRecipe(recipeId++, "Filtered Water", 5f, new[] { ("Water Bottle", 1), ("Cloth", 1) }, 5, "Survival"));
        recipes.Add(CreateWaterRecipe(recipeId++, "Purified Water", 8f, new[] { ("Water Bottle", 1), ("Chemical", 1) }, 10, "Survival"));

        // TOOLS (10 recipes)
        recipes.Add(CreateToolRecipe(recipeId++, "Pickaxe", 15f, new[] { ("Iron Ore", 2), ("Wood Plank", 2) }, 5, "Crafting"));
        recipes.Add(CreateToolRecipe(recipeId++, "Axe", 12f, new[] { ("Iron Ore", 2), ("Wood Plank", 1) }, 5, "Crafting"));
        recipes.Add(CreateToolRecipe(recipeId++, "Shovel", 10f, new[] { ("Iron Ore", 1), ("Wood Plank", 2) }, 3, "Crafting"));
        recipes.Add(CreateToolRecipe(recipeId++, "Fishing Rod", 8f, new[] { ("Wood Plank", 2), ("String", 1) }));
        recipes.Add(CreateToolRecipe(recipeId++, "Lantern", 20f, new[] { ("Metal", 2), ("Glass", 1), ("Oil", 1) }, 10, "Crafting"));

        // ELECTRONICS (15 recipes)
        recipes.Add(CreateElectronicsRecipe(recipeId++, "Light Bulb", 15f, new[] { ("Glass", 1), ("Metal", 1) }, 10, "Electronics"));
        recipes.Add(CreateElectronicsRecipe(recipeId++, "Circuit Board", 20f, new[] { ("Metal", 2), ("Copper", 2) }, 15, "Electronics"));
        recipes.Add(CreateElectronicsRecipe(recipeId++, "Battery", 10f, new[] { ("Copper", 2), ("Zinc", 1) }, 10, "Electronics"));
        recipes.Add(CreateElectronicsRecipe(recipeId++, "Radio", 40f, new[] { ("Circuit Board", 2), ("Metal", 3), ("Battery", 2) }, 25, "Electronics"));
        recipes.Add(CreateElectronicsRecipe(recipeId++, "Generator", 60f, new[] { ("Metal", 5), ("Copper", 3), ("Circuit Board", 2) }, 35, "Electronics"));

        // FURNITURE (10 recipes)
        recipes.Add(CreateFurnitureRecipe(recipeId++, "Wooden Table", 20f, new[] { ("Wood Plank", 6) }));
        recipes.Add(CreateFurnitureRecipe(recipeId++, "Wooden Chair", 15f, new[] { ("Wood Plank", 3) }));
        recipes.Add(CreateFurnitureRecipe(recipeId++, "Bed", 30f, new[] { ("Wood Plank", 5), ("Cloth", 3) }));
        recipes.Add(CreateFurnitureRecipe(recipeId++, "Shelf", 25f, new[] { ("Wood Plank", 5) }));
        recipes.Add(CreateFurnitureRecipe(recipeId++, "Workbench", 40f, new[] { ("Wood Plank", 8), ("Metal", 2) }, 10, "Building"));

        // BUILDING MATERIALS (10 recipes)
        recipes.Add(CreateBuildingRecipe(recipeId++, "Wood Plank", 3f, new[] { ("Wood", 2) }));
        recipes.Add(CreateBuildingRecipe(recipeId++, "Stone Block", 5f, new[] { ("Stone", 1) }));
        recipes.Add(CreateBuildingRecipe(recipeId++, "Metal Plate", 8f, new[] { ("Iron Ore", 2) }, 5, "Crafting"));
        recipes.Add(CreateBuildingRecipe(recipeId++, "Concrete", 10f, new[] { ("Stone", 2), ("Cement", 1) }, 5, "Building"));
        recipes.Add(CreateBuildingRecipe(recipeId++, "Glass Pane", 15f, new[] { ("Glass", 1), ("Metal", 1) }, 10, "Crafting"));

        // TRAPS (10 recipes)
        recipes.Add(CreateTrapRecipe(recipeId++, "Bear Trap", 15f, new[] { ("Metal", 2), ("Wood Plank", 1), ("Chain", 1) }, 10, "Crafting"));
        recipes.Add(CreateTrapRecipe(recipeId++, "Spike Trap", 10f, new[] { ("Metal", 1), ("Wood Plank", 2) }, 8, "Crafting"));
        recipes.Add(CreateTrapRecipe(recipeId++, "Alarm Trap", 20f, new[] { ("Metal", 2), ("Circuit Board", 1), ("Bell", 1) }, 15, "Electronics"));
        recipes.Add(CreateTrapRecipe(recipeId++, "Explosive Trap", 25f, new[] { ("Gunpowder", 2), ("Metal", 2), ("Fuse", 1) }, 20, "Crafting"));

        Debug.Log($"Loaded {recipes.Count} recipes");
    }

    // Helper methods to create recipes
    private Recipe CreateWeaponRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        var recipe = new Recipe
        {
            id = id,
            name = name,
            description = $"Recipe for {name}",
            craftingTime = time,
            requiredSkillLevel = skillLevel,
            requiredSkill = skill,
            isUnlocked = skillLevel == 0
        };

        foreach (var ing in ingredients)
        {
            recipe.ingredients.Add(new Recipe.RecipeIngredient { itemName = ing.Item1, quantity = ing.Item2 });
        }

        return recipe;
    }

    private Recipe CreateArmorRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateMedicalRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateFoodRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateWaterRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateToolRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateElectronicsRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateFurnitureRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateBuildingRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    private Recipe CreateTrapRecipe(int id, string name, float time, (string, int)[] ingredients, int skillLevel = 0, string skill = "")
    {
        return CreateWeaponRecipe(id, name, time, ingredients, skillLevel, skill);
    }

    /// <summary>
    /// Get recipe by ID
    /// </summary>
    public Recipe GetRecipeByID(int id)
    {
        return recipes.FirstOrDefault(r => r.id == id);
    }

    /// <summary>
    /// Get recipe by name
    /// </summary>
    public Recipe GetRecipeByName(string name)
    {
        return recipes.FirstOrDefault(r => r.name == name);
    }

    /// <summary>
    /// Get all recipes
    /// </summary>
    public List<Recipe> GetAllRecipes() => new List<Recipe>(recipes);

    /// <summary>
    /// Get unlocked recipes
    /// </summary>
    public List<Recipe> GetUnlockedRecipes()
    {
        return recipes.Where(r => r.isUnlocked).ToList();
    }
}
