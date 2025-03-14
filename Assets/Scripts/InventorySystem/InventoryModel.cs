using System.Collections.Generic;
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
