namespace FoodOrderingApp.Models
{
    public class ExportInvoiceModel
    {
        public decimal TotalPrice { get; set; }
        public int OrderId { get; set; }
        public List<Basket> Baskets { get; set; }
    }

}
