using BlazorApp23.Components.Models;

namespace BlazorApp23.Components.Services
{
    public class CartService
    {
        private List<CartItem> _cartItems = new();

        public event Action? OnChange;

        public void AddToCart(MenuItem menuItem)
        {
            var existingItem = _cartItems.FirstOrDefault(item => item.MenuItemId == menuItem.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    MenuItemId = menuItem.Id,
                    Name = menuItem.Name,
                    Price = menuItem.Price,
                    DiscountedPrice = menuItem.DiscountedPrice,
                    ImageUrl = menuItem.ImageUrl,
                    Quantity = 1
                });
            }

            NotifyStateChanged();
        }

        public void RemoveFromCart(int menuItemId)
        {
            var item = _cartItems.FirstOrDefault(item => item.MenuItemId == menuItemId);
            if (item != null)
            {
                _cartItems.Remove(item);
                NotifyStateChanged();
            }
        }

        public void UpdateQuantity(int menuItemId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(item => item.MenuItemId == menuItemId);
            if (item != null)
            {
                item.Quantity = quantity;
                NotifyStateChanged();
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
            NotifyStateChanged();
        }

        public List<CartItem> GetCartItems() => _cartItems;

        public decimal GetTotalPrice() =>
            _cartItems.Sum(item => item.DiscountedPrice < item.Price ?
                item.DiscountedPrice * item.Quantity :
                item.Price * item.Quantity);

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class CartItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}