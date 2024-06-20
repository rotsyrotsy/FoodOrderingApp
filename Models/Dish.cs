using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal(3,2)")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [Required]
        public Boolean IsAvailable { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreation { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateUpdate { get; set; }

        public ICollection<Basket> Baskets { get; set; }
    }
}
