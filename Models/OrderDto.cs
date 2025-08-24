using MyShop.Entities;

namespace MyShop.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public required List<OrderItem> Items { get; set; }
        public decimal TotalPriceUSD { get; set; }
    }
}
