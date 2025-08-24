namespace MyShop.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public required List<OrderItem> Items { get; set; }
        public decimal TotalPriceUSD { get; set; }

        public OrderStatus? Status { get; set; }
        public required int StatusId { get; set; }

        public required User User { get; set; }
        public int UserId { get; set; }
    }
}
