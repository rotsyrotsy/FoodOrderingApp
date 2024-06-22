
using FoodOrderingApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Drawing.Printing;

namespace FoodOrderingApp.Data;

public class DishRepository
    {
        private readonly string _connectionString;

        public DishRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FoodOrderingAppContext");
        }
    public async Task<int> GetTotalDishesCountAsync(string searchTerm = null, int? categoryId = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT COUNT(*)
            FROM Dish
            WHERE (@SearchTerm IS NULL OR Name LIKE '%' + @SearchTerm + '%')
            AND (@CategoryId IS NULL OR CategoryId = @CategoryId)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
                command.Parameters.AddWithValue("@CategoryId", (object)categoryId ?? DBNull.Value);

                var totalItems = (int)await command.ExecuteScalarAsync();
                return totalItems;
            }
        }
    }
    public async Task<IEnumerable<Dish>> GetDishesAsync(int pageNumber, int pageSize, string searchTerm = null, int? categoryId = null)
    {
        var dishes = new List<Dish>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT d.Id, d.Name, d.Description, d.Price, d.CategoryId, c.Name AS CategoryName, d.IsAvailable
            FROM Dish d
            JOIN Category c ON d.CategoryId = c.Id
            WHERE (@SearchTerm IS NULL OR d.Name LIKE '%' + @SearchTerm + '%')
            AND (@CategoryId IS NULL OR d.CategoryId = @CategoryId)
            ORDER BY d.Id 
            OFFSET @Offset ROWS 
            FETCH NEXT @PageSize ROWS ONLY";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
                command.Parameters.AddWithValue("@CategoryId", (object)categoryId ?? DBNull.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        dishes.Add(new Dish
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            CategoryId = reader.GetInt32(4),
                            CategoryName = reader.GetString(5),
                            IsAvailable = reader.GetBoolean(6)
                        });
                    }
                }
            }
        }

        return dishes;
    }
    public async Task<Dish> GetDishByIdAsync(int dishId)
    {
        Dish? dish = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
            SELECT d.Id, d.Name, d.Description, d.Price, d.CategoryId, c.Name AS CategoryName, d.IsAvailable
            FROM Dish d
            JOIN Category c ON d.CategoryId = c.Id
            WHERE d.Id = @dishId
            ORDER BY d.Id ";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dishId", dishId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        dish = new Dish
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            CategoryId = reader.GetInt32(4),
                            CategoryName = reader.GetString(5),
                            IsAvailable = reader.GetBoolean(6)
                        };
                    }
                }
            }
        }

        return dish;
    }

    public async Task AddDishAsync(Dish dish)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO Dish (Name, Description, Price) VALUES (@Name, @Description, @Price)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", dish.Name);
                    command.Parameters.AddWithValue("@Description", dish.Description);
                    command.Parameters.AddWithValue("@Price", dish.Price);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }

