namespace MyShop.Models
{
    public class OrderItemGetDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int ShopItemId { get; set; }
        public required decimal Price { get; set; }
        public string CurrencyName { get; set; }
        public required int Quantity { get; set; }
    }
}
