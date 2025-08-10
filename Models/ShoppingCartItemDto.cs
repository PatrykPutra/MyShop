namespace MyShop.Models
{
    public class ShoppingCartItemDto
    {
        public required int ItemId { get; init; }
        public required int Quantity { get; init; }
        public required string Token { get; init; }

    }
}
