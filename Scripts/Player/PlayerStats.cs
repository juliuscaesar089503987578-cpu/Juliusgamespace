using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Player class that manages all player-related data and systems.
/// </summary>
public class Player
{
    private CharacterData characterData;
    private PlayerStats stats;
    private SurvivalSystem survivalSystem;
    private Inventory inventory;
    private SkillTree skillTree;
    private Vector3 position;
    private bool isDead = false;

    public bool IsDead => isDead;
    public CharacterData CharacterData => characterData;
    public PlayerStats Stats => stats;
    public SurvivalSystem SurvivalSystem => survivalSystem;
    public Inventory Inventory => inventory;
    public SkillTree SkillTree => skillTree;
    public Vector3 Position => position;

    /// <summary>
    /// Create a new player with character data
    /// </summary>
    public Player(CharacterData data)
    {
        characterData = data;
        stats = new PlayerStats(data.attributes);
        survivalSystem = new SurvivalSystem(data.profession);
        inventory = new Inventory(100); // 100 kg capacity
        skillTree = new SkillTree();
        position = Vector3.zero;
    }

    /// <summary>
    /// Update player survival stats each frame
    /// </summary>
    public void UpdateSurvivalStats(float deltaTime)
    {
        survivalSystem.Update(deltaTime);

        // Apply survival penalties
        if (survivalSystem.Hunger < 20)
        {
            stats.TakeDamage(2f * deltaTime); // Starving damage
        }

        if (survivalSystem.Thirst < 20)
        {
            stats.TakeStaminaDamage(3f * deltaTime); // Dehydration
        }

        if (survivalSystem.Temperature < 5)
        {
            stats.TakeDamage(1f * deltaTime); // Freezing damage
        }
        else if (survivalSystem.Temperature > 35)
        {
            stats.TakeStaminaDamage(2f * deltaTime); // Heat exhaustion
        }

        // Check for death
        if (stats.Health <= 0)
        {
            isDead = true;
            GameManager.Instance.EventManager.TriggerEvent("PlayerDied");
        }
    }

    /// <summary>
    /// Move player to a position
    /// </summary>
    public void SetPosition(Vector3 newPosition)
    {
        position = newPosition;
    }

    /// <summary>
    /// Get player save data
    /// </summary>
    public CharacterData GetSaveData()
    {
        characterData.playtimeHours = GameManager.Instance.GetGameTime() / 3600f;
        characterData.level = stats.Level;
        characterData.experiencePoints = stats.ExperiencePoints;
        return characterData;
    }
}

/// <summary>
/// Manages all player statistics and attributes.
/// </summary>
public class PlayerStats
{
    private float maxHealth;
    private float health;
    private float maxStamina;
    private float stamina;
    private float armor;
    private int level = 1;
    private int experiencePoints = 0;

    public float Health => health;
    public float MaxHealth => maxHealth;
    public float Stamina => stamina;
    public float MaxStamina => maxStamina;
    public float Armor => armor;
    public int Level => level;
    public int ExperiencePoints => experiencePoints;

    public PlayerStats(CharacterData.Attributes attributes)
    {
        maxHealth = 100 + (attributes.endurance * 5);
        health = maxHealth;
        maxStamina = 100 + (attributes.agility * 3);
        stamina = maxStamina;
        armor = attributes.strength * 2;
    }

    /// <summary>
    /// Take damage (reduced by armor)
    /// </summary>
    public void TakeDamage(float damage)
    {
        float reducedDamage = damage * (1 - (armor / (armor + 100)));
        health -= reducedDamage;
        GameManager.Instance.EventManager.TriggerEvent("PlayerTakeDamage", reducedDamage);
    }

    /// <summary>
    /// Take stamina damage
    /// </summary>
    public void TakeStaminaDamage(float damage)
    {
        stamina -= damage;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
    }

    /// <summary>
    /// Heal the player
    /// </summary>
    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        GameManager.Instance.EventManager.TriggerEvent("PlayerHealed", amount);
    }

    /// <summary>
    /// Restore stamina
    /// </summary>
    public void RestoreStamina(float amount)
    {
        stamina += amount;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
    }

    /// <summary>
    /// Gain experience
    /// </summary>
    public void GainExperience(int amount)
    {
        experiencePoints += amount;
        
        // Check for level up
        int requiredExp = level * 100; // 100 exp per level
        if (experiencePoints >= requiredExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        maxHealth += 10;
        health = maxHealth;
        maxStamina += 10;
        stamina = maxStamina;
        experiencePoints = 0;

        GameManager.Instance.EventManager.TriggerEvent("PlayerLevelUp", level);
    }
}

/// <summary>
/// Manages skill tree and skill progression.
/// </summary>
public class SkillTree
{
    private Dictionary<string, int> skills = new Dictionary<string, int>();

    public SkillTree()
    {
        // Initialize all skills to 0
        string[] skillNames = new[]
        {
            "Melee", "Ranged", "Defense", "CriticalStrike",
            "Crafting", "Weapons", "Armor", "Medicine",
            "Building", "Electricity", "Water", "Defense_Structures",
            "Hunting", "Farming", "Cooking", "Navigation",
            "Medicine_Skill", "Chemistry", "Mechanics", "Electronics",
            "Stealth", "Lockpicking", "Hacking", "Camouflage",
            "Leadership", "Trading", "Morale",
            "Loot_Detection", "Hazard_Resistance",
            "Vehicles", "Survival"
        };

        foreach (string skill in skillNames)
        {
            skills[skill] = 0;
        }
    }

    /// <summary>
    /// Increase skill level
    /// </summary>
    public void IncreaseSkill(string skillName, int amount = 1)
    {
        if (skills.ContainsKey(skillName))
        {
            skills[skillName] = Mathf.Min(skills[skillName] + amount, 100);
        }
    }

    /// <summary>
    /// Get skill level
    /// </summary>
    public int GetSkillLevel(string skillName)
    {
        return skills.ContainsKey(skillName) ? skills[skillName] : 0;
    }
}
