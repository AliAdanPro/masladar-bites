using BlazorApp23.Components.Models;
using Microsoft.Data.SqlClient;

namespace BlazorApp23.Components.Services
{
    public class FoodItemService
    {
        private readonly string _connectionString;

        public FoodItemService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<FoodItem>> GetAllFoodItemsAsync()
        {
            var foodItems = new List<FoodItem>();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT * FROM Foods", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        foodItems.Add(new FoodItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Category = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            DiscountedPrice = reader.GetDecimal(4),
                            ImageUrl = reader.GetString(5),
                            DateAdded = reader.GetDateTime(6)
                        });
                    }
                }
            }
            return foodItems;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = new List<Category>();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT * FROM Categories", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        });
                    }
                }
            }
            return categories;
        }

        public async Task AddFoodItemAsync(FoodItem foodItem)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(
                    "INSERT INTO Foods (Name, Category, Price, DiscountedPrice, ImageUrl) " +
                    "VALUES (@Name, @Category, @Price, @DiscountedPrice, @ImageUrl)",
                    conn);

                cmd.Parameters.AddWithValue("@Name", foodItem.Name);
                cmd.Parameters.AddWithValue("@Category", foodItem.Category);
                cmd.Parameters.AddWithValue("@Price", foodItem.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", foodItem.DiscountedPrice);
                cmd.Parameters.AddWithValue("@ImageUrl", foodItem.ImageUrl);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateFoodItemAsync(FoodItem foodItem)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(
                    "UPDATE Foods SET Name=@Name, Category=@Category, " +
                    "Price=@Price, DiscountedPrice=@DiscountedPrice, ImageUrl=@ImageUrl " +
                    "WHERE Id=@Id",
                    conn);

                cmd.Parameters.AddWithValue("@Id", foodItem.Id);
                cmd.Parameters.AddWithValue("@Name", foodItem.Name);
                cmd.Parameters.AddWithValue("@Category", foodItem.Category);
                cmd.Parameters.AddWithValue("@Price", foodItem.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", foodItem.DiscountedPrice);
                cmd.Parameters.AddWithValue("@ImageUrl", foodItem.ImageUrl);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteFoodItemAsync(int id)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("DELETE FROM Foods WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
