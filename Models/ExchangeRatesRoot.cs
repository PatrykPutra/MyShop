namespace MyShop.Models
{
    public class ExchangeRatesRoot
    {
        public string disclaimer { get; set; } // Nazwy pól z dużej litery. Pewnie masz  zmałej dlatego, że z API dostajesz w małej. Wtedy używasz https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties
        public string license { get; set; }
        public int timestamp { get; set; }
        public string @base { get; set; }
        public Dictionary<string,decimal> rates { get; set; }
        
    }
}
