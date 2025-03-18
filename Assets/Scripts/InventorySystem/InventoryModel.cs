using DataSystem;

namespace InventorySystem
{
    public class InventoryModel
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
    }
}
