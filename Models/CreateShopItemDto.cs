using MyShop.Models;

namespace MyShop.Models
{
    public class CreateShopItemDto
    {
        public required string Name { get; init; }
        public required string Text { get; init; }
        public required decimal PriceUSD { get; init; }
        public required int CategoryId { get; init; }
        public required int Quantity { get; init; }
        public required string Token { get; init; }
    }
}
