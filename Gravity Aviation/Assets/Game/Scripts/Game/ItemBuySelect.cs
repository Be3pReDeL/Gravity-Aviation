using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemBuySelect : MonoBehaviour
{
    public enum ItemType { Skin, Background, Music }
    [SerializeField] private ItemType itemType;
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button buyOrSelectButton;

    private string purchaseKey;
    private string currentKey;
    private bool isPurchased;

    [SerializeField] private Button[] buttons;

    private void Start()
    {
        costText.transform.parent.gameObject.SetActive(true);

        purchaseKey = itemType.ToString() + "_purchased_" + name;
        currentKey = itemType.ToString() + "_current";
        isPurchased = PlayerPrefs.GetInt(purchaseKey, 0) == 1;
        costText.text = cost.ToString();

        if (isPurchased)
        {
            costText.transform.parent.gameObject.SetActive(false); // Уничтожаем текст стоимости, если товар куплен
        }

        UpdateButtonStatus();
    }

    [OPS.Obfuscator.Attribute.DoNotRename]
    public void BuyOrSelect()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = true;

        if (isPurchased)
        {
            SetAsCurrentItem();
        }
        else
        {
            TryBuyItem();
        }
    }

    private void TryBuyItem()
    {
        int totalCoins = PlayerPrefsManager.Instance.GetTotalCoins();
        if (totalCoins >= cost)
        {
            PlayerPrefsManager.Instance.AddCoins(-cost);
            isPurchased = true;
            PlayerPrefs.SetInt(purchaseKey, 1);
            PlayerPrefs.Save();

            SetAsCurrentItem();
            costText.transform.parent.gameObject.SetActive(false);
            UpdateButtonStatus();
        }
        else
        {
            Debug.Log("Not enough coins.");
        }
    }

    private void SetAsCurrentItem()
    {
        PlayerPrefs.SetString(currentKey, name);
        PlayerPrefs.Save();

        // Здесь можно добавить логику для применения выбранного товара

        UpdateButtonStatus();
    }

    private void UpdateButtonStatus()
    {
        bool isCurrentlySelected = PlayerPrefs.GetString(currentKey, "") == name;
        buyOrSelectButton.interactable = !(isPurchased && isCurrentlySelected);
    }
}
