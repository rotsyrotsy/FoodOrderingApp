using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Models
{
    public class Basket
    {
        public int Id { get; set; }
        [Required]
        public int DishId { get; set; }
        [ForeignKey("DishId")]
        public virtual Dish Dish { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int Quantity { get; set; } = 0;

        public String DishName { get; set; }
        public decimal Price { get; set; }
        public String CategoryName { get; set; }
        public decimal BasketPrice { get; set; }

    }
}
