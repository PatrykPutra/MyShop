namespace MyShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int ShopItemId { get; set; }
        public required decimal PriceUSD { get; set; }
    }
}
