using System.ComponentModel.DataAnnotations;
namespace MyShop.Models
{
    public class CreateUserDto
    {
        [MaxLength(25)]
        public required string UserName { get; init; }
        public required bool IsAdmin {  get; init; }
        [MinLength(6)]
        public required string Password { get; init; }
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; init; }
    }
}
