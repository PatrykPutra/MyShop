namespace MyShop.Models
{
    public class Summary
    {
        public required int NumberOfProducts { get; init; }
        public required int NumberOfOrders { get; init; }
        public required int AllProductsQuantity { get; init; }
        public required decimal AllOrdersTotalPriceUSD { get; init; }
    }
}
