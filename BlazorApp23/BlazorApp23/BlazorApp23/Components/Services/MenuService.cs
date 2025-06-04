using BlazorApp23.Components.Models;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BlazorApp23.Components.Services
{
    public class MenuService
    {
        private readonly string _connectionString;

        public MenuService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<MenuItem>> GetAllMenuItemsAsync()
        {
            var menuItems = new List<MenuItem>();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT * FROM Foods", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        menuItems.Add(new MenuItem
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
            return menuItems;
        }

        public async Task<List<MenuCategory>> GetAllMenuCategoriesAsync()
        {
            var categories = new List<MenuCategory>();

            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT * FROM Categories", conn);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new MenuCategory
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

        public async Task AddMenuItemAsync(MenuItem menuItem)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(
                    "INSERT INTO Foods (Name, Category, Price, DiscountedPrice, ImageUrl) " +
                    "VALUES (@Name, @Category, @Price, @DiscountedPrice, @ImageUrl)",
                    conn);

                cmd.Parameters.AddWithValue("@Name", menuItem.Name);
                cmd.Parameters.AddWithValue("@Category", menuItem.Category);
                cmd.Parameters.AddWithValue("@Price", menuItem.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", menuItem.DiscountedPrice);
                cmd.Parameters.AddWithValue("@ImageUrl", menuItem.ImageUrl);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(
                    "UPDATE Foods SET Name=@Name, Category=@Category, " +
                    "Price=@Price, DiscountedPrice=@DiscountedPrice, ImageUrl=@ImageUrl " +
                    "WHERE Id=@Id",
                    conn);

                cmd.Parameters.AddWithValue("@Id", menuItem.Id);
                cmd.Parameters.AddWithValue("@Name", menuItem.Name);
                cmd.Parameters.AddWithValue("@Category", menuItem.Category);
                cmd.Parameters.AddWithValue("@Price", menuItem.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", menuItem.DiscountedPrice);
                cmd.Parameters.AddWithValue("@ImageUrl", menuItem.ImageUrl);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("DELETE FROM Foods WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task PlaceOrderAsync(Order order)
        {
            using (SqlConnection conn = new(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(
                    "INSERT INTO OrderHistory (OrderNumber, Items, TotalPrice, ContactNumber, DeliveryAddress, City) " +
                    "VALUES (@OrderNumber, @Items, @TotalPrice, @ContactNumber, @DeliveryAddress, @City)",
                    conn);

                cmd.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                cmd.Parameters.AddWithValue("@Items", order.SerializedItems);
                cmd.Parameters.AddWithValue("@TotalPrice", order.TotalPrice);
                cmd.Parameters.AddWithValue("@ContactNumber", order.ContactNumber);
                cmd.Parameters.AddWithValue("@DeliveryAddress", order.DeliveryAddress);
                cmd.Parameters.AddWithValue("@City", order.City);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}