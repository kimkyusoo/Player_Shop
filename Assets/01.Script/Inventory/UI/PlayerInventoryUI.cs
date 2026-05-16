using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory; 
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;

    private List<InventorySlot> uiSlots = new List<InventorySlot>();

    public InputAction InventoryAction;

    private bool showingInventory = false;

    void Awake()
    {
        if (inventory == null)
        {
            Debug.LogError($"{gameObject.name}의 PlayerInventoryUI에 Inventory가 연결되지 않았습니다!");
            return;
        }
        GenerateSlots();
    }

    void Start()
    {
        InventoryAction.Enable();

        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshUI;
        }

        inventoryPanel.SetActive(false);
        RefreshUI();
    }

    void Update()
    {
        if (InventoryAction.WasPressedThisFrame())
        {
            ShowInventory();
        }
    }

    public void ShowInventory()
    {
        showingInventory = !showingInventory;

        inventoryPanel.SetActive(showingInventory);
    }

    private void GenerateSlots()
    {
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            GameObject gameObject = Instantiate(slotPrefab, slotParent);
            InventorySlot slot = gameObject.GetComponent<InventorySlot>();
            uiSlots.Add(slot);
        }
    }

    private void RefreshUI()
    {
        if (inventory == null || inventory.dropList == null) return;

        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (uiSlots[i] == null)
            {
                Debug.LogError($"{i}번째 슬롯 스크립트를 찾을 수 없습니다! 프리팹을 확인하세요.");
                continue;
            }

            if (i < inventory.dropList.Count)
                uiSlots[i].UpdateSlot(inventory.dropList[i]);
            else
                uiSlots[i].UpdateSlot(null);
        }
    }
}