using UnityEngine;
using System.Collections.Generic;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string CoinsKey = "coins";

    public static PlayerPrefsManager Instance;

    private const string CurrentLevelKey = "currentLevel";

    const string SkinPrefix = "Skin_";
    const string BackgroundPrefix = "Background_";
    const string MusicPrefix = "Music_";
    const string CurrentSkinKey = "CurrentSkin";
    const string CurrentBackgroundKey = "CurrentBackground";
    const string CurrentMusicKey = "CurrentMusic";


    private const string LeaderboardKey = "leaderboard_";
    private const int MaxLeaderboardEntries = 5;

    public void SaveLevel(int level)
    {
        PlayerPrefs.SetInt(CurrentLevelKey, level);
        PlayerPrefs.Save();
    }

    public int LoadLevel()
    {
        return PlayerPrefs.GetInt(CurrentLevelKey, 1); // Значение по умолчанию - 1
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetTotalCoins()
    {
        return PlayerPrefs.GetInt(CoinsKey, 0);
    }

    public void AddCoins(int amount)
    {
        int coins = GetTotalCoins() + amount;
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }

    public void SavePlayerNameAndMiles(string name, float miles)
    {
        // Сохраняем новую запись
        PlayerPrefs.SetString(LeaderboardKey + (MaxLeaderboardEntries - 1), name + " " + miles.ToString("F1"));

        // Сортируем и обновляем лидерборд
        SortAndUpdateLeaderboard();
    }

    private void SortAndUpdateLeaderboard()
    {
        var entries = new List<string>();
        for (int i = 0; i < MaxLeaderboardEntries; i++)
        {
            if (PlayerPrefs.HasKey(LeaderboardKey + i))
            {
                entries.Add(PlayerPrefs.GetString(LeaderboardKey + i));
            }
        }

        entries.Sort((a, b) => -float.Parse(a.Split(' ')[1]).CompareTo(float.Parse(b.Split(' ')[1])));

        for (int i = 0; i < MaxLeaderboardEntries; i++)
        {
            if (i < entries.Count)
            {
                PlayerPrefs.SetString(LeaderboardKey + i, entries[i]);
            }
        }
    }

    public List<string> GetLeaderboard()
    {
        var leaderboard = new List<string>();
        for (int i = 0; i < MaxLeaderboardEntries; i++)
        {
            if (PlayerPrefs.HasKey(LeaderboardKey + i))
            {
                leaderboard.Add(PlayerPrefs.GetString(LeaderboardKey + i));
            }
        }

        return leaderboard;
    }
}
