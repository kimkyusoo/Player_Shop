using System;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    public string itemName;
    public ItemEffect itemEffect;
    public ItemType itemType;

    public Sprite itemIcon;

    public int quantity = 1;

    public static event Action<DropItem> ItemUsed;
    public void Pickup(PlayerInteraction player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddItem(this);
            gameObject.SetActive(false);
        }
    }

    public string PrintMessage(string message)
    {
        return message;
    }

    public void ExecuteAction()
    {
        ItemUsed.Invoke(this);
    }
}

