namespace MyShop.Entities
{
    public class ShoppingCartItem
    {
        public Guid Id { get; set; }
        public required int ItemId { get; set; }
        public required int Quantity { get; set; }
        public required ShoppingCart ShoppingCart { get; set; }
        public required int ShoppingCartId { get; set; }
    }
}
