namespace MyShop.Models
{
    public class ExchangeRatesRoot
    {
        public string disclaimer { get; set; }
        public string license { get; set; }
        public int timestamp { get; set; }
        public string @base { get; set; }
        public Dictionary<string,decimal> rates { get; set; }
        
    }
}
