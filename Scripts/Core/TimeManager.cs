using UnityEngine;
using System;

/// <summary>
/// Manages game time, day/night cycle, weather, and seasons.
/// </summary>
public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float gameMinuteLength = 2f; // Real seconds per game minute
    [SerializeField] private float startHour = 8f; // Start at 8 AM

    private float worldTime = 0f; // In game minutes
    private int currentDay = 1;
    private int currentSeason = 0; // 0=Spring, 1=Summer, 2=Fall, 3=Winter

    public enum Season { Spring, Summer, Fall, Winter }
    public enum WeatherType { Clear, Rain, Storm, Fog, HeatWave, Snow, RadiationStorm }

    private WeatherType currentWeather = WeatherType.Clear;
    private float weatherTimer = 0f;
    private float weatherDuration = 300f; // 5 minutes

    private void Start()
    {
        worldTime = startHour * 60f; // Convert hours to minutes
    }

    private void Update()
    {
        UpdateTime(Time.deltaTime);
        UpdateWeather(Time.deltaTime);
    }

    /// <summary>
    /// Update world time
    /// </summary>
    public void UpdateTime(float deltaTime)
    {
        worldTime += (deltaTime / gameMinuteLength);

        // Check if day has passed (1440 minutes = 24 hours)
        if (worldTime >= 1440f)
        {
            worldTime -= 1440f;
            currentDay++;
            OnDayPassed();
        }

        GameManager.Instance.EventManager.TriggerEvent("TimeUpdated", GetTimeData());
    }

    /// <summary>
    /// Update weather system
    /// </summary>
    private void UpdateWeather(float deltaTime)
    {
        weatherTimer += deltaTime;

        if (weatherTimer >= weatherDuration)
        {
            ChangeWeather();
            weatherTimer = 0f;
        }
    }

    private void OnDayPassed()
    {
        // Check for season change (7 days per season, 28 days per year)
        if (currentDay % 7 == 0)
        {
            currentSeason = (currentSeason + 1) % 4;
            GameManager.Instance.EventManager.TriggerEvent("SeasonChanged", (Season)currentSeason);
        }

        GameManager.Instance.EventManager.TriggerEvent("DayPassed", currentDay);
    }

    private void ChangeWeather()
    {
        // Simple weather selection based on season
        WeatherType[] seasonWeathers = GetSeasonWeathers((Season)currentSeason);
        int randomIndex = UnityEngine.Random.Range(0, seasonWeathers.Length);
        currentWeather = seasonWeathers[randomIndex];
        weatherDuration = UnityEngine.Random.Range(120f, 600f); // 2-10 minutes

        GameManager.Instance.EventManager.TriggerEvent("WeatherChanged", currentWeather);
    }

    private WeatherType[] GetSeasonWeathers(Season season)
    {
        return season switch
        {
            Season.Spring => new[] { WeatherType.Clear, WeatherType.Rain, WeatherType.Fog },
            Season.Summer => new[] { WeatherType.Clear, WeatherType.HeatWave, WeatherType.Storm },
            Season.Fall => new[] { WeatherType.Clear, WeatherType.Rain, WeatherType.Storm },
            Season.Winter => new[] { WeatherType.Clear, WeatherType.Snow, WeatherType.Storm },
            _ => new[] { WeatherType.Clear }
        };
    }

    public float GetWorldTime() => worldTime;
    public void SetWorldTime(float time) => worldTime = time;
    public int GetCurrentDay() => currentDay;
    public int GetCurrentHour() => (int)(worldTime / 60f);
    public int GetCurrentMinute() => (int)(worldTime % 60f);
    public Season GetCurrentSeason() => (Season)currentSeason;
    public WeatherType GetCurrentWeather() => currentWeather;
    public float GetTemperature() => GetTemperatureBySeason((Season)currentSeason);
    public bool IsNight() => GetCurrentHour() >= 18 || GetCurrentHour() < 6;

    private float GetTemperatureBySeason(Season season)
    {
        return season switch
        {
            Season.Spring => 15f,
            Season.Summer => 30f,
            Season.Fall => 15f,
            Season.Winter => 0f,
            _ => 15f
        };
    }

    public TimeData GetTimeData()
    {
        return new TimeData
        {
            day = currentDay,
            hour = GetCurrentHour(),
            minute = GetCurrentMinute(),
            season = (Season)currentSeason,
            weather = currentWeather,
            temperature = GetTemperature(),
            isNight = IsNight()
        };
    }
}

[System.Serializable]
public struct TimeData
{
    public int day;
    public int hour;
    public int minute;
    public TimeManager.Season season;
    public TimeManager.WeatherType weather;
    public float temperature;
    public bool isNight;
}
