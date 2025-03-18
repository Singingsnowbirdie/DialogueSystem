using DataSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GM_Tools
{
    public class InventoryTool : MonoBehaviour
    {
        private InventoryRepository _inventoryRepository;

        public InventoryRepository InventoryRepository
        {
            get
            {
                _inventoryRepository ??= new InventoryRepository();
                return _inventoryRepository;
            }
        }

        public void ResetData()
        {
            InventoryRepository.ResetData();
        }

        public void SetItemQuantity(string itemId, int quantity)
        {
            List<ItemData> items = InventoryRepository.LoadItemsData();

            ItemData itemData = FindOrCreate(items, itemId);

            itemData.Quantity = quantity;

            InventoryRepository.SaveData(items);
            Debug.Log($"Item {itemId} quantity updated to {quantity}");
        }

        private ItemData FindOrCreate(List<ItemData> items, string itemId)
        {
            ItemData item = items.Find(c => c.ItemID == itemId);

            if (item == null)
            {
                item = new ItemData(itemId);
                items.Add(item);
            }

            return item;
        }

        public void RemoveItem(string itemId)
        {
            List<ItemData> items = InventoryRepository.LoadItemsData();

            ItemData itemToRemove = items.Find(item => item.ItemID == itemId);

            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                Debug.Log($"Item '{itemId}' removed from inventory.");
            }
            else
            {
                Debug.Log($"Item with ID '{itemId}' not found in inventory.");
            }
            InventoryRepository.SaveData(items);
        }
    }
}

