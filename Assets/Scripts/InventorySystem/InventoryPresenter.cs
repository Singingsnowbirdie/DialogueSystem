using DataSystem;
using DialogueSystem.DialogueEditor;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace InventorySystem
{
    public class InventoryPresenter : IStartable
    {
        [Inject] private readonly InventoryModel _model;

        CompositeDisposable _compositeDisposables = new CompositeDisposable();

        public void Start()
        {
            _model.InventoryRepository.LoadData();

            _model.GiveTakeItem
                    .Subscribe(data => GiveTakeItem(data))
                    .AddTo(_compositeDisposables);

            AddItem("Coins", 20); // temp debug
        }

        private void GiveTakeItem(GiveTakeItemData data)
        {
            if (data.GiveTakeEventType == EGiveTakeEventType.Give)
            {
                AddItem(data.ItemID, data.ItemAmount);
            }
            else
            {
                RemoveItem(data.ItemID, data.ItemAmount);
            }
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
