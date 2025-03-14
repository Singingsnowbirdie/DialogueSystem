using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace InventorySystem
{
    public class InventoryModel
    {
        public ReactiveCollection<Item> Items { get; } = new ReactiveCollection<Item>();

        private InventoryRepository _inventoryRepository;

        public InventoryRepository InventoryRepository
        {
            get
            {
                _inventoryRepository ??= new InventoryRepository();
                return _inventoryRepository;
            }
        }

        internal bool TryGetItemByID(string itemId, out Item item)
        {
            Item existingItem = Items.FirstOrDefault(item => item.ItemID == itemId);

            if (existingItem != null)
            {
                item = existingItem;
                return true;
            }
            else
            {
                item = null;
                return false;
            }
        }

        public void LoadInventory()
        {
            List<Item> savedItems = InventoryRepository.LoadInventory();

            Items.Clear();
            foreach (var item in savedItems)
            {
                Items.Add(item);
            }
        }

        public void SaveInventory()
        {
            InventoryRepository.SaveInventory(Items);
        }


    }
}
