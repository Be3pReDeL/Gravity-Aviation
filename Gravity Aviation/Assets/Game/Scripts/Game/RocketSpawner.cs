using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    public Transform spawnPoint; // Точка на сцене, где будет создана ракета
    public GameObject[] rocketPrefabs; // Массив префабов ракет
    public string[] rocketSkinNames; // Массив имен скинов

    private void Start()
    {
        SpawnCurrentRocket();
    }

    private void SpawnCurrentRocket()
    {
        string currentSkin = PlayerPrefs.GetString("Skin_current", "DefaultRocket");

        for (int i = 0; i < rocketSkinNames.Length; i++)
        {
            if (rocketSkinNames[i] == currentSkin && i < rocketPrefabs.Length)
            {
                Instantiate(rocketPrefabs[i], spawnPoint.position, Quaternion.identity);
                return;
            }
        }

        Debug.LogError("Rocket skin not found: " + currentSkin);
    }
}
