using DataSystem;
using VContainer;
using VContainer.Unity;

namespace InventorySystem
{
    public class InventoryPresenter : IStartable
    {
        [Inject] private readonly InventoryModel _model;

        public void Start()
        {
            _model.InventoryRepository.LoadData();

            AddItem("Coins", 20); // temp debug
        }

        public void AddItem(string itemId, int quantity)
        {
            if (quantity < 1)
                return;

            if (_model.InventoryRepository.TryGetItemByID(itemId, out ItemData existingItem))
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _model.InventoryRepository.AddItem(itemId, quantity);
                _model.InventoryRepository.SaveData();
            }
        }

        public void RemoveItem(string itemId, int quantity)
        {
            if (quantity < 1)
                return;

            if (_model.InventoryRepository.TryGetItemByID(itemId, out ItemData existingItem))
            {
                existingItem.Quantity -= quantity;

                if (existingItem.Quantity <= 0)
                    _model.InventoryRepository.RemoveItemById(itemId);
            }

            _model.InventoryRepository.SaveData();
        }
    }
}
