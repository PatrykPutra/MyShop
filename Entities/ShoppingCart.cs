namespace MyShop.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new();
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}