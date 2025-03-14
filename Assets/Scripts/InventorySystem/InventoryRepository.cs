using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryRepository
    {
        private readonly string _saveFilePath;

        public InventoryRepository(string saveFilePath = "inventory.json")
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, saveFilePath);
        }

        public void SaveInventory(IEnumerable<Item> items)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(items, options);
                File.WriteAllText(_saveFilePath, json);
                Debug.Log($"Inventory saved to {_saveFilePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save inventory: {ex.Message}");
            }
        }

        public List<Item> LoadInventory()
        {
            if (!File.Exists(_saveFilePath))
            {
                Debug.LogWarning("No inventory file found. Starting with empty inventory.");
                return new List<Item>();
            }

            try
            {
                string json = File.ReadAllText(_saveFilePath);
                var items = JsonSerializer.Deserialize<List<Item>>(json);
                Debug.Log($"Inventory loaded from {_saveFilePath}");
                return items;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load inventory: {ex.Message}");
                return new List<Item>();
            }
        }

        public void ResetData()
        {
            File.Delete(_saveFilePath);
        }
    }

}
