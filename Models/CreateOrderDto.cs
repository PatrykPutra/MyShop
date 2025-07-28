namespace MyShop.Models
{
    public class CreateOrderDto
    {
        public required List<OrderItemDto> Items { get; set; }
    }
}
