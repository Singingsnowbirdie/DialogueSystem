using InventorySystem;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace GM_Tools
{
    public class InventoryTool : MonoBehaviour
    {
        public ReactiveCollection<Item> Items { get; } = new ReactiveCollection<Item>();

        private InventoryRepository _inventoryRepository;

        private void OnValidate()
        {
            _inventoryRepository ??= new InventoryRepository();
            LoadInventory();
        }

        public void ResetData()
        {
            _inventoryRepository.ResetData();
            Debug.Log("Inventory data has been reset.");
        }

        public void LoadInventory()
        {
            List<Item> savedItems = _inventoryRepository.LoadInventory();

            Items.Clear();
            foreach (var item in savedItems)
            {
                Items.Add(item);
            }
        }

        public void SaveInventory()
        {
            _inventoryRepository.SaveInventory(Items);
        }

        public void AddItem(string itemId, int quantity)
        {
            Item existingItem = Items.FirstOrDefault(item => item.ItemID == itemId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new Item(itemId, quantity));
            }

            SaveInventory();
        }

        public void RemoveItem(string itemId, int quantity)
        {
            var existingItem = Items.FirstOrDefault(item => item.ItemID == itemId);

            if (existingItem != null)
            {
                existingItem.Quantity -= quantity;

                if (existingItem.Quantity <= 0)
                {
                    Items.Remove(existingItem);
                }

                SaveInventory();
            }
            else
            {
                Debug.LogWarning($"Item with Id {itemId} not found in inventory.");
            }
        }
    }
}

