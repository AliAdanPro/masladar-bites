
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp23.Components.Models;

namespace BlazorApp23.Components.Services
{
    public class SalesReportService
    {
        private readonly string _connectionString;

        public SalesReportService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<WeeklyProfit>> GetWeeklySalesAsync(string statusFilter = null)
        {
            var weeklySales = new List<WeeklyProfit>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT 
                        DATEPART(year, OrderDate) AS Year,
                        DATEPART(week, OrderDate) AS WeekNumber,
                        DATEADD(day, -(DATEPART(weekday, OrderDate)-1), CAST(OrderDate AS DATE)) AS WeekStartDate,
                        SUM(TotalPrice) AS TotalSales,
                        COUNT(Id) AS OrderCount,
                        Status
                    FROM OrderHistory
                    {0}
                    GROUP BY DATEPART(year, OrderDate), DATEPART(week, OrderDate),
                             DATEADD(day, -(DATEPART(weekday, OrderDate)-1), CAST(OrderDate AS DATE)),
                             Status
                    ORDER BY Year DESC, WeekNumber DESC";

                var whereClause = string.IsNullOrEmpty(statusFilter)
                    ? string.Empty
                    : "WHERE Status = @Status";

                using (var command = new SqlCommand(string.Format(query, whereClause), connection))
                {
                    if (!string.IsNullOrEmpty(statusFilter))
                    {
                        command.Parameters.AddWithValue("@Status", statusFilter);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            weeklySales.Add(new WeeklyProfit
                            {
                                Year = reader.GetInt32(0),
                                WeekNumber = reader.GetInt32(1),
                                WeekStartDate = reader.GetDateTime(2),
                                TotalSales = reader.GetDecimal(3),
                                OrderCount = reader.GetInt32(4),
                                Status = reader.IsDBNull(5) ? "All" : reader.GetString(5),
                                Profit = reader.GetDecimal(3) * 0.7m 
                            });
                        }
                    }
                }
            }

            return weeklySales;
        }

        public async Task<List<string>> GetOrderStatusesAsync()
        {
            var statuses = new List<string> { "All" };

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT DISTINCT Status FROM OrderHistory WHERE Status IS NOT NULL";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        statuses.Add(reader.GetString(0));
                    }
                }
            }

            return statuses;
        }
    }
}