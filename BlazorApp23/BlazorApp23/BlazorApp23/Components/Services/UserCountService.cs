
using BlazorApp23.Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BlazorApp23.Components.Services
{
    public class UserCountService
    {
        private readonly IConfiguration _configuration;

        public UserCountService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> GetUniqueUserCountAsync()
        {
            try
            {
                using var connection = new SqlConnection(GetConnectionString());
                await connection.OpenAsync();

                using var command = new SqlCommand(
                    "SELECT COUNT(DISTINCT ContactNumber) FROM OrderHistory",
                    connection);

                var result = await command.ExecuteScalarAsync();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user count", ex);
            }
        }

        public async Task<List<OrderHistory>> GetAllOrdersAsync()
        {
            var orders = new List<OrderHistory>();

            try
            {
                using var connection = new SqlConnection(GetConnectionString());
                await connection.OpenAsync();

                using var command = new SqlCommand(
                    "SELECT Id, OrderNumber, Items, TotalPrice, ContactNumber, " +
                    "DeliveryAddress, City, OrderDate, Status FROM OrderHistory",
                    connection);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    orders.Add(new OrderHistory
                    {
                        Id = reader.GetInt32(0),
                        OrderNumber = reader.GetString(1),
                        Items = reader.GetString(2),
                        TotalPrice = reader.GetDecimal(3),
                        ContactNumber = reader.GetString(4),
                        DeliveryAddress = reader.GetString(5),
                        City = reader.GetString(6),
                        OrderDate = reader.GetDateTime(7),
                        Status = reader.IsDBNull(8) ? "Pending" : reader.GetString(8)
                    });
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders", ex);
            }
        }
    }
}