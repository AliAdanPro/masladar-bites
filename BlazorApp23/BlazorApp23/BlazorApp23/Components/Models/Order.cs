using System.Text.Json;
using BlazorApp23.Components.Services;

namespace BlazorApp23.Components.Models
{
    public class Order
    {
        public string OrderNumber { get; set; } = GenerateOrderNumber();
        public List<CartItem> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        
        public string SerializedItems => JsonSerializer.Serialize(Items);

        private static string GenerateOrderNumber()
        {
            var random = new Random();
            return $"ORD-{DateTime.Now:yyyyMMdd}-{random.Next(1000, 9999)}";
        }
    }
}