using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : UIController
{
    public void BuyItem(string itemName, int cost, string categoryPrefix)
    {
        int totalCoins = PlayerPrefsManager.Instance.GetTotalCoins();
        if (totalCoins >= cost && !IsPurchased(itemName, categoryPrefix))
        {
            PlayerPrefsManager.Instance.AddCoins(-cost);
            PlayerPrefs.SetInt(categoryPrefix + itemName, 1); // Установить флаг покупки
            PlayerPrefs.Save();
        }
    }

    public void SelectItem(string itemName, string category)
    {
        if (IsPurchased(itemName, category))
        {
            PlayerPrefs.SetString(category + "_current", itemName);
            PlayerPrefs.Save();
        }
    }

    private bool IsPurchased(string itemName, string categoryPrefix)
    {
        return PlayerPrefs.GetInt(categoryPrefix + itemName, 0) == 1;
    }
}

