using MyShop.Entities;

namespace MyShop.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public required List<OrderItemGetDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrencyName { get; set; }
    }
}
