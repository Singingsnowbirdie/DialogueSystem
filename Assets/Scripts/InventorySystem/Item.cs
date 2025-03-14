namespace InventorySystem
{
    public class Item
    {
        public string ItemID { get; }
        public int Quantity { get; set; }

        public Item(string name, int quantity)
        {
            ItemID = name;
            Quantity = quantity;
        }
    }
}
