namespace MyShop.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int ShopItemId { get; set; }
        public required decimal PriceUSD { get; set; }
        public required int Quantity { get; set; }
    }
}
