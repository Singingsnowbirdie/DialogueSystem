using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace InventorySystem
{
    public class InventoryPresenter : IStartable
    {
        [Inject] private readonly InventoryModel _model;

        public void Start()
        {
            _model.LoadInventory();

            SetStartCoinsAmount();
        }

        private void SetStartCoinsAmount()
        {
            AddItem("Coins", 20);
        }

        public void AddItem(string itemId, int quantity)
        {
            if (quantity < 1)
                return;

            Item existingItem = _model.Items.FirstOrDefault(item => item.ItemID == itemId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _model.Items.Add(new Item(itemId, quantity));
            }

            _model.SaveInventory();
        }

        public void RemoveItem(string itemId, int quantity)
        {
            if (quantity < 1)
                return;

            var existingItem = _model.Items.FirstOrDefault(item => item.ItemID == itemId);

            if (existingItem != null)
            {
                existingItem.Quantity -= quantity;

                if (existingItem.Quantity <= 0)
                {
                    _model.Items.Remove(existingItem);
                }

                _model.SaveInventory();
            }
            else
            {
                Debug.LogWarning($"Item with Id {itemId} not found in inventory.");
            }
        }

    }
}
