using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Dish> Dishes { get; set; }
    }
}
