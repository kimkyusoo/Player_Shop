using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Serializable]
    public struct ShopItem
    {
        public string itemName;
        public int price;
        public Sprite itemImage;
    }
    public ShopItem[] items;
    [SerializeField] private PlayerInventory purchaseTarget;

    public void BuyItem(int index)
    {
        Debug.Log($"구매 시도: {index}번 아이템");
        if (index < 0 || index >= items.Length) return;

        if (purchaseTarget == null) return;


        ShopItem buyItem = items[index];

        if (purchaseTarget.CheckHasCoin(buyItem.price))
        {
            Debug.Log($"[Shop] 아이템: {buyItem.itemName}, 가격: {buyItem.price}, 내 코인: {purchaseTarget.hasCoin}, 구매가능여부: {purchaseTarget.CheckHasCoin(buyItem.price)}");
            purchaseTarget.SpendCoin(buyItem.price);
            purchaseTarget.AddItem(buyItem.itemName, 1); 

            ShopUI.Instance.UpdateCoinUI(); 
            ShopUI.Instance.ShowFeedback($"{buyItem.itemName} Purchase Success!", Color.green);
        }
        else
        {
            int lackAmount = buyItem.price - purchaseTarget.hasCoin;
            ShopUI.Instance.ShowFeedback($"Purchase Fail. Insufficient credits: {lackAmount}", Color.red);
        }
    }
}
