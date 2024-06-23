using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal(20,2)")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public string? CategoryName { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public Boolean IsAvailable { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreation { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateUpdate { get; set; } = DateTime.Now;

        public ICollection<Basket>? Baskets { get; set; }
    }
}
