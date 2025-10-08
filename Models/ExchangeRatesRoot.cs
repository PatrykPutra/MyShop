namespace MyShop.Models
{
    public class ExchangeRatesRoot
    {
        public string Disclaimer { get; set; } // Nazwy pól z dużej litery. Pewnie masz  zmałej dlatego, że z API dostajesz w małej. Wtedy używasz https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties
        public string License { get; set; }
        public int Timestamp { get; set; }
        public string @Base { get; set; }
        public Dictionary<string,decimal> Rates { get; set; }
        
    }
}
