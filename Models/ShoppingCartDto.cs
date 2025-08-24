namespace MyShop.Models
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public List<ShopItemDto> ShopItems { get; set; } = new List<ShopItemDto>();
    }
}
