using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Models
{
    public enum OrderState
    {
        Placed=0,
        Confirmed=1,
        Preparing=2,
        OutForDelivery=3,
        Delivered=4,
        Cancelled=5
    }
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required]
        public OrderState state { get; set; } = OrderState.Placed;
        public string? Address { get; set; }
        public ICollection<Basket>? Baskets { get; set; }
    }
}
