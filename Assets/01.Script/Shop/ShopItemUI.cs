using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;

    private int itemIndex;
    private Shop currentShop;

    public void Setup(Shop shop, int index, Shop.ShopItem item)
    {
        currentShop = shop;
        itemIndex = index;

        itemNameText.text = item.itemName;
        itemPriceText.text = $"{item.price} Coin";
        itemIcon.sprite = item.itemImage;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => currentShop.BuyItem(itemIndex));
    }
}