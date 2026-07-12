using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Represents character data and customization options.
/// Serializable for save/load functionality.
/// </summary>
[System.Serializable]
public class CharacterData
{
    [System.Serializable]
    public class Appearance
    {
        public int gender; // 0 = Male, 1 = Female
        public int facePreset;
        public int hairstyle;
        public int hairColor;
        public int eyeColor;
        public int skinTone;
        public int beardStyle; // Male only
        public int clothingPreset;
        public int armorType; // 0 = None, 1 = Light, 2 = Medium, 3 = Heavy
        public int accessories; // Bitmask for multiple accessories
        public int backpack;
    }

    [System.Serializable]
    public class Attributes
    {
        public int strength = 10;
        public int agility = 10;
        public int intelligence = 10;
        public int endurance = 10;
        public int charisma = 10;

        public int GetTotal() => strength + agility + intelligence + endurance + charisma;
    }

    public string characterName;
    public int profession; // 0-7 (Medic, Engineer, Hunter, etc.)
    public Appearance appearance;
    public Attributes attributes;
    public float playtimeHours;
    public int experiencePoints;
    public int level = 1;

    public CharacterData()
    {
        appearance = new Appearance();
        attributes = new Attributes();
        playtimeHours = 0f;
        experiencePoints = 0;
    }
}

/// <summary>
/// Manages character creation UI and data flow.
/// </summary>
public class CharacterCreator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas characterCreationCanvas;
    [SerializeField] private Transform characterPreviewArea;

    private CharacterData currentCharacterData;
    private int currentStep = 0;
    private const int TOTAL_STEPS = 5; // Name, Appearance, Profession, Attributes, Confirm

    public enum CreationStep
    {
        NameAndGender,
        Appearance,
        Profession,
        Attributes,
        Confirmation
    }

    private void Start()
    {
        currentCharacterData = new CharacterData();
    }

    /// <summary>
    /// Start character creation process
    /// </summary>
    public void StartCharacterCreation()
    {
        currentCharacterData = new CharacterData();
        currentStep = 0;
        ShowStep(CreationStep.NameAndGender);
    }

    /// <summary>
    /// Show a specific step in character creation
    /// </summary>
    public void ShowStep(CreationStep step)
    {
        currentStep = (int)step;
        // UI implementation would happen here
        Debug.Log($"Character Creation Step: {step}");
    }

    /// <summary>
    /// Move to next step
    /// </summary>
    public void NextStep()
    {
        if (currentStep < TOTAL_STEPS - 1)
        {
            currentStep++;
            ShowStep((CreationStep)currentStep);
        }
    }

    /// <summary>
    /// Move to previous step
    /// </summary>
    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            ShowStep((CreationStep)currentStep);
        }
    }

    /// <summary>
    /// Set character name
    /// </summary>
    public void SetCharacterName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning("Character name cannot be empty");
            return;
        }
        currentCharacterData.characterName = name;
    }

    /// <summary>
    /// Set character gender
    /// </summary>
    public void SetGender(int gender)
    {
        currentCharacterData.appearance.gender = gender;
        UpdatePreview();
    }

    /// <summary>
    /// Set face preset
    /// </summary>
    public void SetFacePreset(int preset)
    {
        currentCharacterData.appearance.facePreset = preset;
        UpdatePreview();
    }

    /// <summary>
    /// Set hairstyle
    /// </summary>
    public void SetHairstyle(int hairstyle)
    {
        currentCharacterData.appearance.hairstyle = hairstyle;
        UpdatePreview();
    }

    /// <summary>
    /// Set hair color
    /// </summary>
    public void SetHairColor(int color)
    {
        currentCharacterData.appearance.hairColor = color;
        UpdatePreview();
    }

    /// <summary>
    /// Set eye color
    /// </summary>
    public void SetEyeColor(int color)
    {
        currentCharacterData.appearance.eyeColor = color;
        UpdatePreview();
    }

    /// <summary>
    /// Set skin tone
    /// </summary>
    public void SetSkinTone(int tone)
    {
        currentCharacterData.appearance.skinTone = tone;
        UpdatePreview();
    }

    /// <summary>
    /// Set beard style (male only)
    /// </summary>
    public void SetBeardStyle(int style)
    {
        if (currentCharacterData.appearance.gender == 0) // Male
        {
            currentCharacterData.appearance.beardStyle = style;
            UpdatePreview();
        }
    }

    /// <summary>
    /// Set starting profession
    /// </summary>
    public void SetProfession(int profession)
    {
        currentCharacterData.profession = profession;
        ApplyProfessionBonuses(profession);
    }

    /// <summary>
    /// Apply profession bonuses to attributes
    /// </summary>
    private void ApplyProfessionBonuses(int profession)
    {
        // Reset to base
        currentCharacterData.attributes.strength = 10;
        currentCharacterData.attributes.agility = 10;
        currentCharacterData.attributes.intelligence = 10;
        currentCharacterData.attributes.endurance = 10;
        currentCharacterData.attributes.charisma = 10;

        // Apply profession bonuses
        switch (profession)
        {
            case 0: // Medic
                currentCharacterData.attributes.intelligence += 3;
                currentCharacterData.attributes.endurance += 2;
                break;
            case 1: // Engineer
                currentCharacterData.attributes.intelligence += 4;
                currentCharacterData.attributes.agility += 1;
                break;
            case 2: // Hunter
                currentCharacterData.attributes.agility += 3;
                currentCharacterData.attributes.endurance += 2;
                break;
            case 3: // Soldier
                currentCharacterData.attributes.strength += 3;
                currentCharacterData.attributes.endurance += 2;
                break;
            case 4: // Farmer
                currentCharacterData.attributes.strength += 2;
                currentCharacterData.attributes.intelligence += 2;
                break;
            case 5: // Mechanic
                currentCharacterData.attributes.intelligence += 3;
                currentCharacterData.attributes.strength += 1;
                break;
            case 6: // Scientist
                currentCharacterData.attributes.intelligence += 4;
                currentCharacterData.attributes.agility += 1;
                break;
            case 7: // Scavenger
                currentCharacterData.attributes.agility += 3;
                currentCharacterData.attributes.charisma += 1;
                break;
        }
    }

    /// <summary>
    /// Set individual attribute
    /// </summary>
    public void SetAttribute(int attributeType, int value)
    {
        value = Mathf.Clamp(value, 5, 20); // Min 5, Max 20
        
        switch (attributeType)
        {
            case 0:
                currentCharacterData.attributes.strength = value;
                break;
            case 1:
                currentCharacterData.attributes.agility = value;
                break;
            case 2:
                currentCharacterData.attributes.intelligence = value;
                break;
            case 3:
                currentCharacterData.attributes.endurance = value;
                break;
            case 4:
                currentCharacterData.attributes.charisma = value;
                break;
        }
    }

    /// <summary>
    /// Finalize character and start game
    /// </summary>
    public void FinalizeCharacter()
    {
        if (string.IsNullOrWhiteSpace(currentCharacterData.characterName))
        {
            Debug.LogWarning("Please enter a character name");
            return;
        }

        GameManager.Instance.CreateNewGame(currentCharacterData);
        Debug.Log($"Character created: {currentCharacterData.characterName}");
    }

    /// <summary>
    /// Update character preview
    /// </summary>
    private void UpdatePreview()
    {
        // This would update the 3D/2D preview in the UI
        Debug.Log("Character preview updated");
    }

    public CharacterData GetCurrentCharacterData() => currentCharacterData;
}
