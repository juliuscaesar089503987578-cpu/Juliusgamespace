using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Event manager for decoupled communication between systems.
/// Uses a dictionary of delegates for event subscriptions.
/// </summary>
public class EventManager : MonoBehaviour
{
    private Dictionary<string, Delegate> eventDictionary = new Dictionary<string, Delegate>();

    /// <summary>
    /// Subscribe to an event
    /// </summary>
    public void Subscribe(string eventName, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = null;
        }

        eventDictionary[eventName] = (Action)eventDictionary[eventName] + listener;
    }

    /// <summary>
    /// Subscribe to an event with data
    /// </summary>
    public void Subscribe<T>(string eventName, Action<T> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = null;
        }

        eventDictionary[eventName] = (Action<T>)eventDictionary[eventName] + listener;
    }

    /// <summary>
    /// Unsubscribe from an event
    /// </summary>
    public void Unsubscribe(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = (Action)eventDictionary[eventName] - listener;
        }
    }

    /// <summary>
    /// Unsubscribe from an event with data
    /// </summary>
    public void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = (Action<T>)eventDictionary[eventName] - listener;
        }
    }

    /// <summary>
    /// Trigger an event
    /// </summary>
    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            Action action = eventDictionary[eventName] as Action;
            action?.Invoke();
        }
    }

    /// <summary>
    /// Trigger an event with data
    /// </summary>
    public void TriggerEvent<T>(string eventName, T data)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            Action<T> action = eventDictionary[eventName] as Action<T>;
            action?.Invoke(data);
        }
    }
}
