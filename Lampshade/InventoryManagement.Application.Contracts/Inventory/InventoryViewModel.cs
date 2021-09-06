namespace InventoryManagement.Application.Contracts.Inventory
{
    public class InventoryViewModel
    {
        public long Id { get; set; }
        public string Prodcut { get; set; }
        public long ProdcutId { get; set; }
        public double UnitPrice { get; set; }
        public bool InStock { get; set; }
        public long CurrentCount { get; set; }
    }
}
