using System.ComponentModel.DataAnnotations;

namespace FoodOrderingApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        public User User { get; set; }
    }
}
