using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    public Image itemIcon;      
    public Button slotButton;
    public TextMeshProUGUI qtyText;

    private DropItem currentItem;

    private PlayerInventory inventory;

    void Awake()
    {
        if(inventory == null) inventory = FindFirstObjectByType<PlayerInventory>();
        slotButton.onClick.AddListener(OnSlotClicked);
    }

    public void UpdateSlot(DropItem item)
    {
        currentItem = item;

        if (item != null)
        {
            itemIcon.gameObject.SetActive(true);

            itemIcon.sprite = item.itemIcon;

            if(item.quantity > 1)
            {
                qtyText.gameObject.SetActive(true);
                Debug.Log($"UpdateSlot 호출 확인");
                qtyText.text = item.quantity.ToString();
                Debug.Log($"QTY Text: {qtyText.text}");
            }
            else
            {
                qtyText.gameObject.SetActive(false);
            }

            Color color = itemIcon.color;
            color.a = 1f;
            itemIcon.color = color;

        }
        else
        {
            itemIcon.gameObject.SetActive(false);
            itemIcon.sprite = null;
            qtyText.gameObject.SetActive(false);
        }
    }

    public void OnSlotClicked()
    {
        if (inventory == null) return;

        inventory.SetSelectedItem(currentItem);
        
    }
}