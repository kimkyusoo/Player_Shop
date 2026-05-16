using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance; 

    [Header("UI Panels")]
    public GameObject shopPanel;      
    public Transform contentParent;   

    [Header("Display Elements")]
    public TextMeshProUGUI playerCoinText;
    public TextMeshProUGUI feedbackText;

    [Header("Prefabs")]
    public GameObject itemSlotPrefab; 

    private PlayerInventory currentPlayer;

    public PlayerInventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        shopPanel.SetActive(false); 
    }

    public void OpenShop(Shop shop, PlayerInventory player)
    {
        shopPanel.SetActive(true);
        currentPlayer = player;

        if (inventoryUI != null)
        {
            inventoryUI.ShowInventory();
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < shop.items.Length; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, contentParent);
            ShopItemUI slot = slotObj.GetComponent<ShopItemUI>();
            slot.Setup(shop, i, shop.items[i]);
        }

        UpdateCoinUI();
        ShowFeedback("What are you looking for?", Color.white);
    }

    public void UpdateCoinUI()
    {
        if (currentPlayer != null && playerCoinText != null)
        {
            playerCoinText.text = $"HasCoin: {currentPlayer.hasCoin}";
        }
    }

    public void ShowFeedback(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (inventoryUI != null)
        {
            inventoryUI.ShowInventory();
        }
    }
}