
using FoodOrderingApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Drawing.Printing;

namespace FoodOrderingApp.Data;

public class BasketRepository
    {
        private readonly string _connectionString;

        public BasketRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FoodOrderingAppContext");
        }

    public async Task AddBasketAsync(Basket basket)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO Basket (DishId, OrderId, Quantity) VALUES (@DishId, @OrderId, @Quantity)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DishId", basket.DishId);
                    command.Parameters.AddWithValue("@OrderId", basket.OrderId);
                    command.Parameters.AddWithValue("@Quantity", basket.Quantity);
                 

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    public async Task<List<Basket>> GetBasketsByOrderIdAsync(int orderId)
    {
        var baskets = new List<Basket>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT b.Id, b.DishId, b.Quantity, d.Name as DishName, d.Price, c.Name as CategoryName
            FROM Basket b
            JOIN Dish d ON d.Id = b.DishId
            JOiN Category c ON c.Id = d.CategoryId
            WHERE b.OrderId = @orderId
            ORDER BY b.Id DESC";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@orderId", orderId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        baskets.Add(new Basket
                        {
                            Id = reader.GetInt32(0),
                            DishId = reader.GetInt32(1),
                            Quantity = reader.GetInt32(2),
                            DishName = reader.GetString(3),
                            Price = reader.GetDecimal(4),
                            CategoryName = reader.GetString(5),
                         
                        });
                    }
                }
            }
        }

        return baskets;
    }
}

