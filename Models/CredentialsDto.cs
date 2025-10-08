using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class CredentialsDto
    {
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; init; }
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; init; }
    }
}
