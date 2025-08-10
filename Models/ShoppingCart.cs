namespace MyShop.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public List<int> ShopItemsIds { get; set; } = new List<int>();
        public User User { get; set; }
        public int UserId { get; set; }
    }
}