using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MilkShake;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ShakePreset shakePreset;

    public int lives = 5; // Количество жизней игрока
    public Image[] livesImages; // Массив UI элементов для отображения жизней
    public GameObject deathScreen, deathPrefab; // Экран смерти

    public int currentGameCoins = 0; // Монеты, собранные за текущую игру

    public TextMeshProUGUI coinsText, gameOverCoinsText; // Ссылка на TextMeshPro для отображения монет
    
    public TextMeshProUGUI milesText, gameOverMilestText; // TextMeshPro для отображения миль
    public TextMeshProUGUI levelButton;
    public float milesPerSecond = 1f; // Количество миль, которое прибавляется за секунду

    private float totalMiles = 0f;

    public float timeScaleIncrease = 0.1f; // Увеличение timeScale за определенный промежуток времени
    public float timeScaleIncreaseInterval = 5f; // Интервал времени (в секундах), через который увеличивается timeScale

    private float timeSinceLastIncrease = 0f;

    public LevelGenerator levelGenerator;

    public TMP_InputField playerNameInputField; // Ссылка на поле ввода имени

    public void SavePlayerName()
    {
        string playerName = playerNameInputField.text;
        PlayerPrefsManager.Instance.SavePlayerNameAndMiles(playerName, totalMiles);
    }
    
    private void Start()
    {
        Shaker.ShakeAll(shakePreset);
        UpdateCoinsText(); // Инициализируем текст при старте игры
    }

    private void Update()
    {
        if (RocketController.Instance.enabled) {
            totalMiles += milesPerSecond * Time.deltaTime;
            UpdateMilesText();
            timeSinceLastIncrease += Time.deltaTime;
            if (timeSinceLastIncrease >= timeScaleIncreaseInterval) {
                Time.timeScale += timeScaleIncrease;
                timeSinceLastIncrease = 0f;
            }  
        }     
    }

    private void GameOver()
    {
        deathScreen.SetActive(true);
        levelGenerator.enabled = false; // Отключаем генератор уровня

        UpdateMilesTextAtGameOver();
        UpdateCoinsTextAtGameOver();

        Instantiate(deathPrefab, RocketController.Instance.transform.position, quaternion.identity);

        Destroy(RocketController.Instance.gameObject);

        PlayerPrefsManager.Instance.SaveLevel(PlayerPrefsManager.Instance.LoadLevel() + 1);

        levelButton.text = "Level " + PlayerPrefsManager.Instance.LoadLevel().ToString();

        Time.timeScale = 1f; // Сбрасываем timeScale при проигрыше
    }

    private void UpdateMilesTextAtGameOver()
    {
        if (milesText != null)
        {
            gameOverMilestText.text = "Total Miles: " + totalMiles.ToString("F1");
        }
    }

    private void UpdateCoinsTextAtGameOver()
    {
        if (coinsText != null)
        {
            gameOverCoinsText.text = "Coins Collected: " + currentGameCoins.ToString();
        }
    }

    private void UpdateMilesText()
    {
        milesText.text = totalMiles.ToString("F1") + " Miles"; // Округляем до одного знака после запятой
    }

    public void CollectCoin()
    {
        currentGameCoins++;
        PlayerPrefsManager.Instance.AddCoins(1);
        UpdateCoinsText();
    }

    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = currentGameCoins.ToString();
        }
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

    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--;
            livesImages[lives].gameObject.SetActive(false);

            if (lives == 0)
            {
                GameOver();
            }
        }
    }
}
