using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;

    private void Start()
    {
        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI()
    {
        var leaderboard = PlayerPrefsManager.Instance.GetLeaderboard();
        leaderboardText.text = "Leaderboard:\n\n";
        foreach (var entry in leaderboard)
        {
            leaderboardText.text += $"{entry}\n";
        }
    }
}
