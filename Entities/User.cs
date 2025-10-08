using Microsoft.AspNetCore.Identity;

namespace MyShop.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; init; }
        public required bool IsAdmin { get; set; }
        public string? PasswordHash { get; set; }
        public required string Email { get; set; }
        public required ShoppingCart Cart { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();


    }
}
