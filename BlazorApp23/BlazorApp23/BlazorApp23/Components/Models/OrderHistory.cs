
namespace BlazorApp23.Components.Models
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Items { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}