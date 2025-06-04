namespace BlazorApp23.Components.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}