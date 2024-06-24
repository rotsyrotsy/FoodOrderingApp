namespace FoodOrderingApp.Models
{
    public class ExportInvoiceModel
    {
        public decimal TotalPrice { get; set; }
        public int OrderId { get; set; }
        public List<Basket> Baskets { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
    }

}
