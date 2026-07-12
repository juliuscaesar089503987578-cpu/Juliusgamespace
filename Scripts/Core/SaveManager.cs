using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Manages saving and loading of game data.
/// Supports multiple save slots with auto-save functionality.
/// </summary>
public class SaveManager : MonoBehaviour
{
    private const string SAVE_PATH = "/SaveData/";
    private const string SAVE_EXTENSION = ".sav";
    private const string AUTO_SAVE_NAME = "autosave";

    private float autoSaveInterval = 300f; // 5 minutes
    private float autoSaveTimer = 0f;

    private void Start()
    {
        // Create save directory if it doesn't exist
        string path = Application.persistentDataPath + SAVE_PATH;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private void Update()
    {
        // Auto-save every 5 minutes
        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= autoSaveInterval)
        {
            AutoSave();
            autoSaveTimer = 0f;
        }
    }

    /// <summary>
    /// Save the game to a named file
    /// </summary>
    public void SaveGame(string saveName)
    {
        try
        {
            Player player = GameManager.Instance.GetCurrentPlayer();
            if (player == null)
            {
                Debug.LogError("No active player to save");
                return;
            }

            GameSaveData saveData = new GameSaveData
            {
                playerData = player.GetSaveData(),
                worldTime = GameManager.Instance.TimeManager.GetWorldTime(),
                playtime = GameManager.Instance.GetGameTime(),
                timestamp = System.DateTime.Now.ToString()
            };

            string path = GetSavePath(saveName);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, saveData);
            }

            Debug.Log($"Game saved: {saveName}");
            GameManager.Instance.EventManager.TriggerEvent("GameSaved", saveName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    /// <summary>
    /// Load the game from a named file
    /// </summary>
    public void LoadGame(string saveName)
    {
        try
        {
            string path = GetSavePath(saveName);
            if (!File.Exists(path))
            {
                Debug.LogError($"Save file not found: {saveName}");
                return;
            }

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                GameSaveData saveData = (GameSaveData)formatter.Deserialize(fs);

                // Restore player
                Player player = new Player(saveData.playerData);
                GameManager.Instance.CreateNewGame(saveData.playerData);

                // Restore world state
                GameManager.Instance.TimeManager.SetWorldTime(saveData.worldTime);

                Debug.Log($"Game loaded: {saveName}");
                GameManager.Instance.EventManager.TriggerEvent("GameLoaded", saveName);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
        }
    }

    /// <summary>
    /// Auto-save the current game
    /// </summary>
    private void AutoSave()
    {
        SaveGame(AUTO_SAVE_NAME);
    }

    /// <summary>
    /// Get all available save files
    /// </summary>
    public string[] GetAvailableSaves()
    {
        string path = Application.persistentDataPath + SAVE_PATH;
        if (!Directory.Exists(path))
            return new string[0];

        string[] files = Directory.GetFiles(path, "*" + SAVE_EXTENSION);
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }

        return files;
    }

    /// <summary>
    /// Delete a save file
    /// </summary>
    public void DeleteSave(string saveName)
    {
        try
        {
            string path = GetSavePath(saveName);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Save deleted: {saveName}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete save: {e.Message}");
        }
    }

    private string GetSavePath(string saveName)
    {
        return Application.persistentDataPath + SAVE_PATH + saveName + SAVE_EXTENSION;
    }
}

[System.Serializable]
public class GameSaveData
{
    public CharacterData playerData;
    public float worldTime;
    public float playtime;
    public string timestamp;
}
