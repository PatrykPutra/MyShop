namespace MyShop.Models
{
    public class OrderDto
    {
        public required List<OrderItemDto> Items { get; set; }
    }
}
