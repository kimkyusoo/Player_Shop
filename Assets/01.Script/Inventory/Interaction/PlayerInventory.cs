using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    public List<DropItem> itemPrefabs = new List<DropItem>();
    public string[] canDropItems = { "Potion", "Gun", "Shose" };

    public List<DropItem> dropList = new List<DropItem>();
    public Queue<string> pickupMessages = new Queue<string>();
    public DropItem selectedItem;

    public InputAction MessageAction;
    public InputAction InventoryCheckAction;
    public InputAction UseAction;

    public int inventorySize = 9;
    public int hasCoin = 100;

    public static event Action<ItemEffect> effectApplied;
    public event Action OnInventoryChanged;

    private void Start()
    {
        MessageAction.Enable();
        InventoryCheckAction.Enable();
        UseAction.Enable();
    }

    private void Update()
    {
        if (MessageAction.WasPressedThisFrame())
        {
            PrintMessage();
        }

        if (InventoryCheckAction.WasPressedThisFrame())
        {
            PrintInventory();
        }

        if (UseAction.WasPressedThisFrame() && selectedItem != null)
        {
            selectedItem.ExecuteAction();
        }
    }

    private void OnEnable()
    {
        DropItem.ItemUsed += RequestUseItem;
    }

    private void OnDisable()
    {
        // 메모리 누수 방지를 위해 구독 해제
        DropItem.ItemUsed -= RequestUseItem;
    }

    // 인벤토리 내용 전체 출력
    void PrintInventory()
    {
        Debug.Log("===== 현재 인벤토리 소지 아이템 내역 =====");
        for (int i = 0; i < dropList.Count; i++)
        {
            Debug.Log($"소지 아이템: {dropList[i].itemName}, 효과: {dropList[i].itemEffect.ToString()}");
        }
        Debug.Log($"현재 아이템 수: {dropList.Count}");
        Debug.Log("==========================================");
    }

    // 아이템 넣기
    public void AddItem(DropItem item)
    {
        if (!IsValidItemId(item.itemName))
        {
            Debug.LogWarning($"등록되지 않은 아이템 ID입니다. {item.itemName}");
            return;
        }

        DropItem hasItem = dropList.Find(x => x.itemName == item.itemName);
        Debug.Log("hasItem: " + hasItem);
        if (hasItem != null)
        {
            hasItem.quantity++;
            Debug.Log("Qty: " + hasItem.quantity);
            //Destroy(hasItem.gameObject);
        }
        else
        {
            if (dropList.Count < inventorySize)
            {
                dropList.Add(item);
                item.transform.SetParent(this.transform);
                item.gameObject.SetActive(false);
            }
        }
        FinishAddingItem(hasItem != null ? hasItem.itemName : item.itemName);
    }

    public void AddItem(string itemName, int quantity)
    {
        DropItem hasItem = dropList.Find(x => x.itemName == itemName);

        if (hasItem != null)
        {
            hasItem.quantity += quantity;
        }
        else
        {
            DropItem prefab = itemPrefabs.Find(x => x.itemName == itemName);

            if (prefab != null && dropList.Count < inventorySize)
            {
                DropItem newItem = Instantiate(prefab, this.transform);
                newItem.itemName = itemName; 
                newItem.quantity = quantity;
                newItem.gameObject.SetActive(false); 

                dropList.Add(newItem);
            }
            else
            {
                return;
            }
        }

        FinishAddingItem(itemName);
    }

    private void FinishAddingItem(string itemName)
    {
        OnInventoryChanged?.Invoke();
        pickupMessages.Enqueue(itemName + " 획득!");
        Debug.Log($"[Inventory] {itemName} 처리 완료.");
        PrintInventory();
    }

    private bool IsValidItemId(string targetId)
    {
        for (int i = 0; i < canDropItems.Length; i++)
        {
            if (canDropItems[i] == targetId)
            {
                return true;
            }
        }
        return false;
    }

    public void SetSelectedItem(DropItem item)
    {
        selectedItem = item;
        Debug.Log($"현재 선택된 아이템: {(item != null ? item.itemName : "없음")}");
    }

    void PrintMessage()
    {
        if (pickupMessages.Count <= 0)
        {
            Debug.Log("처리할 메세지가 없습니다."); return;
        }

        string result = pickupMessages.Dequeue();
        Debug.Log($"[Message] {result}");
    }

    private void RequestUseItem(DropItem item)
    {
        UseItem(item);
    }

    // 아이템 사용
    public bool UseItem(DropItem dropItem)
    {
        if (dropItem == null) return false;
        if (!dropList.Contains(dropItem)) return false;

        ApplyItemEffect(dropItem.itemEffect);

        dropItem.quantity--;

        if(dropItem.quantity <= 0)
        {
            dropList.Remove(dropItem);
        }

        OnInventoryChanged?.Invoke();

        return true;

    }
    
    private void ApplyItemEffect(ItemEffect effect)
    {
        effectApplied?.Invoke(effect);
    }

    public bool CheckHasCoin(int requiredAmount)
    {
        return hasCoin >= requiredAmount;
    }

    public void SpendCoin(int requiredAmount)
    {
        hasCoin -= requiredAmount;
    }
}
