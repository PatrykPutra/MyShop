namespace MyShop.Models
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Rate { get; set; }
        public required DateTime Updated { get; set; }
    }
}
