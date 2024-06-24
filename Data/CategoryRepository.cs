
using FoodOrderingApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Drawing.Printing;

namespace FoodOrderingApp.Data;

public class CategoryRepository
{
    private readonly string _connectionString;

    public CategoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("FoodOrderingAppContext");
    }
    public async Task<IEnumerable<Category>> GetCategoryesAsync(int pageNumber, int pageSize, string searchTerm = null)
    {
        var categoryes = new List<Category>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
                SELECT d.Id, d.Name
                FROM Category d
                WHERE (@SearchTerm IS NULL OR d.Name LIKE '%' + @SearchTerm + '%')
                ORDER BY d.Id 
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categoryes.Add(new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),

                        });
                    }
                }
            }
        }

        return categoryes;
    }
}
   

