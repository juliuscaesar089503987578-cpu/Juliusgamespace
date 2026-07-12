using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Main game manager that controls all core systems and game flow.
/// Implements singleton pattern for global access.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    private GameState currentGameState = GameState.Menu;
    private Player currentPlayer;
    private float gameTime = 0f;

    [Header("Systems")]
    public SaveManager SaveManager { get; private set; }
    public EventManager EventManager { get; private set; }
    public TimeManager TimeManager { get; private set; }
    public InventorySystem InventorySystem { get; private set; }
    public CraftingSystem CraftingSystem { get; private set; }
    public NPCManager NPCManager { get; private set; }
    public ExperienceSystem ExperienceSystem { get; private set; }

    public enum GameState
    {
        Menu,
        CharacterCreation,
        Loading,
        Playing,
        Paused,
        Dead,
        GameOver
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize all systems
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        SaveManager = GetComponent<SaveManager>();
        EventManager = GetComponent<EventManager>();
        TimeManager = GetComponent<TimeManager>();
        InventorySystem = GetComponent<InventorySystem>();
        CraftingSystem = GetComponent<CraftingSystem>();
        NPCManager = GetComponent<NPCManager>();
        ExperienceSystem = GetComponent<ExperienceSystem>();

        if (SaveManager == null) gameObject.AddComponent<SaveManager>();
        if (EventManager == null) gameObject.AddComponent<EventManager>();
        if (TimeManager == null) gameObject.AddComponent<TimeManager>();
        if (InventorySystem == null) gameObject.AddComponent<InventorySystem>();
        if (CraftingSystem == null) gameObject.AddComponent<CraftingSystem>();
        if (NPCManager == null) gameObject.AddComponent<NPCManager>();
        if (ExperienceSystem == null) gameObject.AddComponent<ExperienceSystem>();
    }

    private void Update()
    {
        if (currentGameState == GameState.Playing)
        {
            gameTime += Time.deltaTime;
            UpdateGameLoop();
        }
    }

    private void UpdateGameLoop()
    {
        if (currentPlayer != null)
        {
            // Update all systems
            TimeManager.UpdateTime(Time.deltaTime);
            currentPlayer.UpdateSurvivalStats(Time.deltaTime);
            
            // Check for death
            if (currentPlayer.IsDead)
            {
                ChangeGameState(GameState.Dead);
            }
        }
    }

    /// <summary>
    /// Change the current game state
    /// </summary>
    public void ChangeGameState(GameState newState)
    {
        if (currentGameState == newState) return;

        GameState oldState = currentGameState;
        currentGameState = newState;

        EventManager.TriggerEvent("GameStateChanged", new GameStateChangeData { oldState = oldState, newState = newState });

        HandleStateChange(newState);
    }

    private void HandleStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                Time.timeScale = 0f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Dead:
                Time.timeScale = 0f;
                break;
        }
    }

    /// <summary>
    /// Create a new game with character data
    /// </summary>
    public void CreateNewGame(CharacterData characterData)
    {
        currentPlayer = new Player(characterData);
        InventorySystem.InitializeInventory(currentPlayer);
        ExperienceSystem.InitializePlayer(currentPlayer);
        
        ChangeGameState(GameState.Playing);
        EventManager.TriggerEvent("GameStarted");
    }

    /// <summary>
    /// Load a saved game
    /// </summary>
    public void LoadGame(string saveName)
    {
        SaveManager.LoadGame(saveName);
        ChangeGameState(GameState.Loading);
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        if (currentGameState == GameState.Playing)
        {
            ChangeGameState(GameState.Paused);
        }
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        if (currentGameState == GameState.Paused)
        {
            ChangeGameState(GameState.Playing);
        }
    }

    public GameState GetGameState() => currentGameState;
    public Player GetCurrentPlayer() => currentPlayer;
    public float GetGameTime() => gameTime;
}

public struct GameStateChangeData
{
    public GameManager.GameState oldState;
    public GameManager.GameState newState;
}
