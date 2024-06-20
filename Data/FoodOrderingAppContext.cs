using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FoodOrderingApp.Models;

namespace FoodOrderingApp.Data
{
    public class FoodOrderingAppContext : DbContext
    {
        public FoodOrderingAppContext (DbContextOptions<FoodOrderingAppContext> options)
            : base(options)
        {
        }

        public DbSet<FoodOrderingApp.Models.Dish> Dish { get; set; } = default!;
        public DbSet<FoodOrderingApp.Models.Category> Category { get; set; } = default!;
        public DbSet<FoodOrderingApp.Models.Order> Order { get; set; } = default!;
        public DbSet<FoodOrderingApp.Models.Basket> Basket { get; set; } = default!;
        public DbSet<FoodOrderingApp.Models.Restaurant> Restaurant { get; set; } = default!;
        public DbSet<FoodOrderingApp.Models.Role> Role { get; set; } = default!;
        public DbSet<FoodOrderingApp.Models.User> User { get; set; } = default!;
    }
}
