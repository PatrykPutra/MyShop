namespace MyShop.Models
{
    public class ShopItemDto
    {
        public required string Name { get; set; }
        public required string Text { get; set; }
        public required decimal Price { get; set; }
        public required string PriceCurrency {  get; set; }
        public required int CategoryId { get; set; }
        public required int Quantity { get; set; }
    }
}
