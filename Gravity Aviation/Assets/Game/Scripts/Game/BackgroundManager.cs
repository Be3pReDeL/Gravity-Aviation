using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Sprite[] backgrounds; // Массив спрайтов для фонов
    private Image backgroundRenderer; // SpriteRenderer для отображения фона

    private void Start()
    {
        backgroundRenderer = GetComponent<Image>();
        ApplyCurrentBackground();
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public void ApplyCurrentBackground()
    {
        string currentBackground = PlayerPrefs.GetString("Background_current", "defaultBackground");
        Sprite selectedBackground = FindBackgroundByName(currentBackground);
        if (selectedBackground != null)
        {
            backgroundRenderer.sprite = selectedBackground;
        }
    }

    private Sprite FindBackgroundByName(string name)
    {
        foreach (Sprite bg in backgrounds)
        {
            if (bg.name == name)
                return bg;
        }
        return null; // Возвращает null, если фон не найден
    }
}
