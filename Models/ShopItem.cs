using System.Security.Principal;

namespace MyShop.Models
{
    public class ShopItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Text { get; set; }
        public required decimal PriceUSD { get; set; }
        public required int Quantity { get; set; } 

        public required int CategoryId { get; set; }
        public required ItemCategory Category { get; set; }
        
    }
}
