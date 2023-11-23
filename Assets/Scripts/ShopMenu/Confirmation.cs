using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Confirmation : MonoBehaviour
{
	public Button yesButton;
	public Button noButton;
	public TextMeshProUGUI messageText;
	private ShopManager shopManager;
	private GameManager gameManager;
	private int itemID;

	void Start()
	{
		gameManager = GameManager.Instance;
		shopManager = FindObjectOfType<ShopManager>();
		yesButton.onClick.AddListener(YesClicked);
		noButton.onClick.AddListener(NoClicked);
		gameObject.SetActive(false);
	}

	public void OpenConfirmationWindow(string message, int itemID)
	{
		this.itemID = itemID; // Store the item ID for later use
		messageText.text = message;
		gameObject.SetActive(true);
	}

	private void YesClicked()
	{
		gameObject.SetActive(false);
		int itemPrice = shopManager.shopItems[2, itemID];
		gameManager.SetCoins(gameManager.GetCoins() - itemPrice);
		shopManager.ConisTXT.text = "Coins: " + gameManager.GetCoins().ToString();
		shopManager.DisableItemButton(itemID);
		GameManager.Instance.AvailableSpells[shopManager.itemNames[itemID]] = true;
		gameManager.SaveCheckpoint();
	}

	private void NoClicked()
	{
		gameObject.SetActive(false);
	}
}
