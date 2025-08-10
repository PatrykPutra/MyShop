namespace MyShop.Models
{
    public class Token
    {
        public int Id { get; set; }
        public required string Body { get; set; }
        public required User User { get; set; }
        public required int UserId { get; set; }

    }
}
