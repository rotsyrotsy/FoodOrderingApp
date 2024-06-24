
using FoodOrderingApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Drawing.Printing;

namespace FoodOrderingApp.Data;

public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
             _connectionString = configuration.GetConnectionString("FoodOrderingAppContext");
        }
    public async Task<IEnumerable<Order>> GetOrderByUserAsync(int userId)
    {
        var orders = new List<Order>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT o.Id, o.UserId, o.Date, o.state, o.Address
            FROM [Order] o
            WHERE o.UserId = @userId
            ORDER BY o.Id DESC";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            state = reader.GetInt32(3),
                            Address = reader.GetString(4)
                            
                        });
                    }
                }
            }
        }

        return orders;
    }
    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        Order ord = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT o.Id, o.UserId, o.Date, o.state, o.Address
            FROM [Order] o
            WHERE o.Id = @orderId
            ORDER BY o.Id DESC";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@orderId", orderId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ord = new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            Date = reader.GetDateTime(2),
                            state = reader.GetInt32(3),
                            Address = reader.GetString(4)

                        };
                    }
                }
            }
        }

        return ord;
    }

    public async Task<int> AddOrderAsync(Order order)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            INSERT INTO [Order] (UserId, Date, state, Address) 
            VALUES (@UserId, @Date, @state, @Address);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", order.UserId);
                command.Parameters.AddWithValue("@Date", order.Date);
                command.Parameters.AddWithValue("@state", order.state);
                command.Parameters.AddWithValue("@Address", order.Address);

                // Execute the command and get the inserted order ID
                var orderId = (int)await command.ExecuteScalarAsync();
                return orderId;
            }
        }
    }
}

