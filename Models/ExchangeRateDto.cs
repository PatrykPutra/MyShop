namespace MyShop.Models
{
    public class ExchangeRateDto
    {
        public required string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
