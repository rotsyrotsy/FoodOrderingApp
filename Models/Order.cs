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

        public static OrderState getCorrespondingState(int state)
        {
            switch (state)
            {
                case 0:return OrderState.Placed;
                case 1: return OrderState.Confirmed;
                case 2: return OrderState.Preparing;
                case 3: return OrderState.OutForDelivery;
                case 4: return OrderState.Delivered;
                case 5: return OrderState.Cancelled;
                default: return OrderState.Placed;
            }
           
        }
    }
}
