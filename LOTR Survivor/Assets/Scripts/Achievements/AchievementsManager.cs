using System;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public bool IsUnlocked { get; private set; }

    public Achievement(string id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        IsUnlocked = false;
    }

    public void Unlock()
    {
        if (!IsUnlocked)
        {
            IsUnlocked = true;
            Debug.Log($"Succès débloqué : {Name} - {Description}");
        }
    }
}

public class AchievementsManager
{
    private static AchievementsManager _instance;
    public static AchievementsManager Instance => _instance ??= new AchievementsManager();

    private readonly Dictionary<string, Achievement> _achievements;

    private AchievementsManager()
    {
        _achievements = new Dictionary<string, Achievement>();
        InitializeAchievements();
        LoadAchievements();
    }

    private void InitializeAchievements()
    {
        AddAchievement(new Achievement("first_kill", "Première élimination", "Tu as éliminé ton premier ennemi."));
        AddAchievement(new Achievement("100_gold", "Riche !", "Tu as collecté 100 pièces d'or."));
    }

    private void AddAchievement(Achievement achievement)
    {
        if (!_achievements.ContainsKey(achievement.Id))
        {
            _achievements.Add(achievement.Id, achievement);
        }
    }

    public void UnlockAchievement(string id)
    {
        if (_achievements.TryGetValue(id, out Achievement achievement))
        {
            achievement.Unlock();
            SaveAchievements();
        }
    }

    public bool IsUnlocked(string id)
    {
        return _achievements.TryGetValue(id, out Achievement a) && a.IsUnlocked;
    }

    private void SaveAchievements()
    {
        foreach (var pair in _achievements)
        {
            PlayerPrefs.SetInt($"achievement_{pair.Key}", pair.Value.IsUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        foreach (var pair in _achievements)
        {
            int unlocked = PlayerPrefs.GetInt($"achievement_{pair.Key}", 0);
            if (unlocked == 1)
            {
                pair.Value.Unlock();
            }
        }
    }
}
