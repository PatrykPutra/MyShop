namespace MyShop.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
