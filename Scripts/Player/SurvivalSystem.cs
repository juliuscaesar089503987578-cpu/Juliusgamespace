using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages player survival stats: hunger, thirst, fatigue, temperature, diseases.
/// </summary>
public class SurvivalSystem
{
    // Core survival stats (0-100)
    private float hunger = 100f;
    private float thirst = 100f;
    private float fatigue = 100f;
    private float temperature = 20f;
    private float morale = 75f;

    // Status conditions
    private bool isBleeding = false;
    private bool isInfected = false;
    private bool hasBrokenBones = false;
    private float poisonAmount = 0f;
    private float radiationAmount = 0f;
    private float sleepDeprived = 0f;

    // Drain rates based on profession
    private float hungerDrainRate = 0.5f; // Per second
    private float thirstDrainRate = 1f;
    private float fatigueDrainRate = 0.3f;

    public float Hunger => hunger;
    public float Thirst => thirst;
    public float Fatigue => fatigue;
    public float Temperature => temperature;
    public float Morale => morale;
    public bool IsBleeding => isBleeding;
    public bool IsInfected => isInfected;
    public bool HasBrokenBones => hasBrokenBones;
    public float PoisonAmount => poisonAmount;
    public float RadiationAmount => radiationAmount;

    public SurvivalSystem(int profession)
    {
        // Adjust drain rates based on profession
        switch (profession)
        {
            case 0: // Medic - more resistant to disease
                break;
            case 1: // Engineer - normal
                break;
            case 2: // Hunter - better at surviving
                hungerDrainRate *= 0.8f;
                thirstDrainRate *= 0.8f;
                break;
            case 3: // Soldier - higher endurance
                fatigueDrainRate *= 0.7f;
                break;
            case 4: // Farmer - better food usage
                hungerDrainRate *= 0.7f;
                break;
        }
    }

    /// <summary>
    /// Update survival stats each frame
    /// </summary>
    public void Update(float deltaTime)
    {
        // Drain survival stats
        hunger -= hungerDrainRate * deltaTime;
        thirst -= thirstDrainRate * deltaTime;
        fatigue -= fatigueDrainRate * deltaTime;
        
        // Clamp to valid ranges
        hunger = Mathf.Clamp(hunger, 0, 100);
        thirst = Mathf.Clamp(thirst, 0, 100);
        fatigue = Mathf.Clamp(fatigue, 0, 100);

        // Apply status condition damage
        if (isBleeding)
        {
            GameManager.Instance.GetCurrentPlayer().Stats.TakeDamage(2f * deltaTime);
        }

        if (isInfected)
        {
            GameManager.Instance.GetCurrentPlayer().Stats.TakeDamage(1f * deltaTime);
        }

        if (poisonAmount > 0)
        {
            GameManager.Instance.GetCurrentPlayer().Stats.TakeDamage(poisonAmount * deltaTime);
            poisonAmount -= 1f * deltaTime;
            poisonAmount = Mathf.Max(poisonAmount, 0);
        }

        if (radiationAmount > 0)
        {
            GameManager.Instance.GetCurrentPlayer().Stats.TakeDamage(1f * deltaTime);
            radiationAmount -= 0.5f * deltaTime;
            radiationAmount = Mathf.Max(radiationAmount, 0);
        }

        // Update morale based on conditions
        UpdateMorale(deltaTime);
    }

    private void UpdateMorale(float deltaTime)
    {
        // Decrease morale if conditions are bad
        if (hunger < 30) morale -= 5f * deltaTime;
        if (thirst < 30) morale -= 5f * deltaTime;
        if (fatigue < 20) morale -= 3f * deltaTime;
        if (isBleeding || isInfected) morale -= 2f * deltaTime;

        // Recover morale in good conditions
        if (hunger > 70 && thirst > 70 && fatigue > 70)
        {
            morale += 2f * deltaTime;
        }

        morale = Mathf.Clamp(morale, 0, 100);
    }

    /// <summary>
    /// Eat food
    /// </summary>
    public void Eat(float amount, float quality = 1f)
    {
        hunger += amount * quality;
        hunger = Mathf.Clamp(hunger, 0, 100);
        GameManager.Instance.EventManager.TriggerEvent("PlayerAte", amount);
    }

    /// <summary>
    /// Drink water
    /// </summary>
    public void Drink(float amount, float quality = 1f)
    {
        thirst += amount * quality;
        thirst = Mathf.Clamp(thirst, 0, 100);
        GameManager.Instance.EventManager.TriggerEvent("PlayerDrank", amount);
    }

    /// <summary>
    /// Sleep to restore fatigue
    /// </summary>
    public void Sleep(float hours)
    {
        fatigue += hours * 15f; // 15 fatigue per hour of sleep
        fatigue = Mathf.Clamp(fatigue, 0, 100);
        sleepDeprived = 0f;
        GameManager.Instance.EventManager.TriggerEvent("PlayerSlept", hours);
    }

    /// <summary>
    /// Apply poison
    /// </summary>
    public void ApplyPoison(float amount)
    {
        poisonAmount += amount;
    }

    /// <summary>
    /// Apply radiation
    /// </summary>
    public void ApplyRadiation(float amount)
    {
        radiationAmount += amount;
    }

    /// <summary>
    /// Start bleeding
    /// </summary>
    public void StartBleeding()
    {
        isBleeding = true;
        GameManager.Instance.EventManager.TriggerEvent("StatusConditionApplied", "Bleeding");
    }

    /// <summary>
    /// Stop bleeding
    /// </summary>
    public void StopBleeding()
    {
        isBleeding = false;
        GameManager.Instance.EventManager.TriggerEvent("StatusConditionRemoved", "Bleeding");
    }

    /// <summary>
    /// Apply infection
    /// </summary>
    public void ApplyInfection()
    {
        isInfected = true;
        GameManager.Instance.EventManager.TriggerEvent("StatusConditionApplied", "Infection");
    }

    /// <summary>
    /// Cure infection
    /// </summary>
    public void CureInfection()
    {
        isInfected = false;
        GameManager.Instance.EventManager.TriggerEvent("StatusConditionRemoved", "Infection");
    }

    /// <summary>
    /// Break bones
    /// </summary>
    public void BreakBones()
    {
        hasBrokenBones = true;
        GameManager.Instance.EventManager.TriggerEvent("StatusConditionApplied", "BrokenBones");
    }

    /// <summary>
    /// Heal broken bones
    /// </summary>
    public void HealBones()
    {
        hasBrokenBones = false;
        GameManager.Instance.EventManager.TriggerEvent("StatusConditionRemoved", "BrokenBones");
    }

    /// <summary>
    /// Update temperature (affected by weather)
    /// </summary>
    public void SetTemperature(float temp)
    {
        temperature = temp;
    }
}
