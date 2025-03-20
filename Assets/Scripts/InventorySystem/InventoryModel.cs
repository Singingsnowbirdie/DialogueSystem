using DataSystem;
using DialogueSystem.DialogueEditor;
using UniRx;

namespace InventorySystem
{
    public class InventoryModel
    {
        public ISubject<GiveTakeItemData> GiveTakeItem { get; } = new Subject<GiveTakeItemData>();

        private InventoryRepository _inventoryRepository;

        public InventoryRepository InventoryRepository
        {
            get
            {
                _inventoryRepository ??= new InventoryRepository();
                return _inventoryRepository;
            }
        }
    }

    public readonly struct GiveTakeItemData
    {
        public GiveTakeItemData(string itemID, int itemAmount, EGiveTakeEventType giveTakeEventType)
        {
            ItemID = itemID;
            ItemAmount = itemAmount;
            GiveTakeEventType = giveTakeEventType;
        }

        public string ItemID { get; }
        public int ItemAmount { get; }
        public EGiveTakeEventType GiveTakeEventType { get; }
    }
}
