using UnityEngine;
using TMPro;

public class CoinsDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance; // Assuming GameManager is a Singleton

        if (coinsText != null && gameManager != null)
        {
            UpdateCoinsDisplay();
        }
        else
        {
            Debug.LogWarning("CoinsDisplay: Missing references to coinsText or gameManager.");
        }
    }

    private void UpdateCoinsDisplay()
    {
        if (coinsText != null && gameManager != null)
        {
            coinsText.text = "Coins: " + gameManager.getCoins().ToString();
        }
    }

}
