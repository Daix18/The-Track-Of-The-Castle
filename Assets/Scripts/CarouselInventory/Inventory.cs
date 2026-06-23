using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> _inventorySlot = new List<InventorySlot>();

    public void AddItem(ItemData itemData, int quantity)
    {
        InventorySlot slot = _inventorySlot.Find(s => s._itemData == itemData);
        if (slot != null)
        {
            slot._quantity += quantity;
        }
        else
        {
            InventorySlot newSlot = new InventorySlot();
            newSlot._itemData = itemData;
            newSlot._quantity = quantity;
            _inventorySlot.Add(newSlot);
        }
    }

    public void RemoveItem(ItemData itemData, int quantity)
    {
        InventorySlot slot = _inventorySlot.Find(s => s._itemData == itemData);

        if (slot != null)
        {
            slot._quantity -= quantity;
            if (slot._quantity <= 0)
            {
                _inventorySlot.Remove(slot);
            }
        }
    }
}